module Store.Crm

open System
open Microsoft.FSharp.Reflection

open Store.Shared.DomainModel
open FSharp.Json.Json

open Amazon.DynamoDBv2
open Amazon.DynamoDBv2.DocumentModel

type DynamoDbTableItem<'a> = { key: string; sort: string; data: 'a }

let dynamoDbClient = new AmazonDynamoDBClient()

let genericQuery<'a> (tableName: string) (query: QueryFilter) : 'a seq =
    let table = Table.LoadTable(dynamoDbClient, tableName)
    let query = table.Query(query)

    query.GetRemainingAsync()
    |> Async.AwaitTask
    |> Async.RunSynchronously
    |> Seq.map _.ToJson()
    |> Seq.map deserialize<DynamoDbTableItem<'a>>
    |> Seq.map _.data

let genericScan<'a> (tableName: string) : 'a seq =
    let table = Table.LoadTable(dynamoDbClient, tableName)
    let scan = table.Scan(new ScanOperationConfig())

    scan.GetRemainingAsync()
    |> Async.AwaitTask
    |> Async.RunSynchronously
    |> Seq.map _.ToJson()
    |> Seq.map deserialize<DynamoDbTableItem<'a>>
    |> Seq.map _.data

let genericPut<'a> (tableName: string) (item: DynamoDbTableItem<'a>) : unit =
    let table = Table.LoadTable(dynamoDbClient, tableName)
    serialize item |> Document.FromJson |> table.PutItemAsync |> Async.AwaitTask |> Async.RunSynchronously |> ignore

let genericDelete<'a> (tableName: string) (item: DynamoDbTableItem<'a>) : unit =
    let table = Table.LoadTable(dynamoDbClient, tableName)
    serialize item |> Document.FromJson |> table.DeleteItemAsync |> Async.AwaitTask |> Async.RunSynchronously |> ignore

/// <summary>Gets all interactions of the specified type.</summary>
/// <param name="interactionType">Use <see langword="nameof" /> to get this value.</param>
let interactions (interactionType: string) : Interaction seq =
    if FSharpType.GetUnionCases(typeof<InteractionType>) |> Array.exists (fun i -> i.Name = interactionType) then
        genericQuery<Interaction> "Interactions" (QueryFilter("key", QueryOperator.Equal, interactionType))
    else
        Seq.empty

let addInteraction (interaction: Interaction) =
    let key =
        match interaction with
        | ClientInteraction(it, _)
        | CompanyInteraction(it, _) -> FSharpValue.GetUnionFields(it, typeof<InteractionType>) |> fst |> _.Name

    let sort =
        match interaction with
        | ClientInteraction(_, dto)
        | CompanyInteraction(_, dto) -> dto.ToString("o")

    genericPut "Interactions" { key = key; sort = sort; data = interaction }

let payments () : PaymentPlan seq =
    genericScan<PaymentPlan> "Payments"

let paymentsByCustomer (customer: CorrelationId) : PaymentPlan seq =
    let query = new QueryFilter("sort", QueryOperator.Equal, customer)
    genericQuery<PaymentPlan> "Payments" query

let paymentByOrder (order: string) : PaymentPlan option =
    let query = new QueryFilter("key", QueryOperator.Equal, order)
    genericQuery<PaymentPlan> "Payments" query |> Seq.tryExactlyOne

let addPayment (payment: PaymentPlan) =
    let key = payment.Order |> snd |> _.OrderNumber.ToString()
    let sort = payment.Customer.CorrelationId
    genericPut "Payments" { key = key; sort = sort; data = payment }

let updatePayment (orderNumber: string) : PaymentUpdate -> PaymentPlan option =
    match
        genericQuery<PaymentPlan> "Payments" (new QueryFilter("key", QueryOperator.Equal, orderNumber)) |> Seq.tryHead
    with
    | Some existing ->
        function
        | TransactionMade transaction ->
            let newPayment = { existing with Transactions = transaction :: existing.Transactions }
            addPayment newPayment
            Some newPayment
        | StatusUpdated state ->
            let newState = (state, DateTimeOffset.UtcNow) :: (fst existing.Order)
            let newPayment = { existing with Order = (newState, snd existing.Order) }
            addPayment newPayment
            Some newPayment
        | TrackingNumberCreated trackingNum ->
            let newData = { snd existing.Order with TrackingNumber = Some trackingNum }
            let newPayment = { existing with Order = (fst existing.Order, newData) }
            addPayment newPayment
            Some newPayment
        | CustomerDataUpdated customer ->
            let newPayment = { existing with Customer = customer }
            addPayment newPayment
            Some newPayment

    | None -> fun _ -> None

let leads (since: DateTimeOffset, until: DateTimeOffset) : Lead seq =
    let query =
        QueryFilter("sort", QueryOperator.GreaterThanOrEqual, since.ToString("o"))

    query.AddCondition("sort", QueryOperator.LessThanOrEqual, until.ToString("o"))
    genericQuery<Lead> "Leads" query

let addLead (lead: Lead) =
    let key = lead.CorrelationId
    let sort = lead.Impression.Time.ToString("o")
    genericPut "Leads" { key = key; sort = sort; data = lead }

let tryFindCustomer (id: CorrelationId) : Customer option =
    let paymentCustomers =
        payments () |> Seq.map _.Customer |> Seq.distinct |> Seq.toList

    paymentCustomers |> List.tryFind (fun c -> c.CorrelationId = id)

let stock () : Stock =
    genericScan<Product * Quantity> "Stock" |> Seq.filter (snd >> fun (Quantity q) -> q > 0)

let catalogue () : Catalogue =
    genericScan<Product * Quantity> "Stock" |> Seq.map fst

let createProduct (product: Product) : Result<Product, string> =
    let (key, sort) = (product.Name, product.Collection.Name)
    let filter = QueryFilter("key", QueryOperator.Equal, key)
    filter.AddCondition("sort", QueryOperator.Equal, sort)

    if genericQuery<Product * Quantity> "Stock" filter |> Seq.isEmpty then
        genericPut "Stock" { key = key; sort = sort; data = (product, OutOfStock) }
        Ok product
    else
        Error "Product already exists."

let updateProduct (product: Product) : Result<Quantity, string> =
    let (key, sort) = (product.Name, product.Collection.Name)
    let filter = QueryFilter("key", QueryOperator.Equal, key)
    filter.AddCondition("sort", QueryOperator.Equal, sort)

    match genericQuery<Product * Quantity> "Stock" filter |> Seq.toList with
    | [ (_, q) ] ->
        genericPut "Stock" { key = key; sort = sort; data = (product, q) }
        Ok q
    | [] -> Error "Product does not exist."
    | _ -> Error "More than one product exists with that name and collection."

let deleteProduct (product: Product) : Result<Quantity, string> =
    let (key, sort) = (product.Name, product.Collection.Name)
    let filter = QueryFilter("key", QueryOperator.Equal, key)
    filter.AddCondition("sort", QueryOperator.Equal, sort)

    match genericQuery<Product * Quantity> "Stock" filter |> Seq.toList with
    | [ (_, q) ] ->
        genericDelete "Stock" { key = key; sort = sort; data = (product, q) }
        Ok q
    | [] -> Error "Product does not exist."
    | _ -> Error "More than one product exists with that name and collection."

let updateStockQuantity (product: Product) (quantity: Quantity) : Result<unit, string> =
    let (key, sort) = (product.Name, product.Collection.Name)
    let filter = QueryFilter("key", QueryOperator.Equal, key)
    filter.AddCondition("sort", QueryOperator.Equal, sort)

    match genericQuery<Product * Quantity> "Stock" filter |> Seq.toList with
    | [ (p, _) ] ->
        genericPut "Stock" { key = key; sort = sort; data = (p, quantity) }
        Ok()
    | [] -> Error "Product does not exist."
    | _ -> Error "More than one product exists with that name and collection."
