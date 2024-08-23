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

let genericPut<'a> (tableName: string) (item: DynamoDbTableItem<'a>) : unit =
    let table = Table.LoadTable(dynamoDbClient, tableName)
    serialize item |> Document.FromJson |> table.PutItemAsync |> Async.AwaitTask |> Async.RunSynchronously |> ignore

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


let payments (since: DateTimeOffset, until: DateTimeOffset) : PaymentPlan seq =
    let query =
        QueryFilter("sort", QueryOperator.GreaterThanOrEqual, since.ToString("o"))

    query.AddCondition("sort", QueryOperator.LessThanOrEqual, until.ToString("o"))
    genericQuery<PaymentPlan> "Payments" query

let addPayment (payment: PaymentPlan) =
    let key = payment.Customer.CorrelationId
    let sort = payment.Order |> snd |> _.OrderNumber.ToString()
    genericPut "Payments" { key = key; sort = sort; data = payment }

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
        payments(DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
        |> Seq.map _.Customer
        |> Seq.distinct
        |> Seq.toList

    paymentCustomers |> List.tryFind (fun c -> c.CorrelationId = id)