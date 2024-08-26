namespace Store.Lambdas.Stripe

open Amazon.Lambda.Core
open FSharp.Data
open Amazon.Lambda.APIGatewayEvents
open Store.Shared.DomainModel
open Store.Crm
open System.Net


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer>)>]
()

type PaymentIntent = JsonProvider<"webhook-sample.json">

type Function() =
    member __.FunctionHandler (request: APIGatewayHttpApiV2ProxyRequest) (_: ILambdaContext) =
        try
            let payment = PaymentIntent.Parse request.Body

            let amount =
                match payment.Currency.ToUpperInvariant() with
                | "USD" -> USD
                | "CAD" -> CAD
                | "RMB" -> RMB
                | _ ->
                    raise
                    <| new System.NotSupportedException("Unsupported currency " + payment.Currency.ToUpperInvariant())
                <| (decimal payment.Amount / 100m)

            let nameBits = payment.Shipping.Name.Split()
            let orderNumber = payment.Metadata.OrderNumber

            match paymentByOrder orderNumber with
            | Some order ->
                let address =
                    match payment.Shipping.Address.Country.ToUpperInvariant() with
                    | "US" ->
                        UsStreetAddress {
                            StreetAddress = payment.Shipping.Address.Line1
                            Unit =
                                if System.String.IsNullOrWhiteSpace payment.Shipping.Address.Line2 then
                                    None
                                else
                                    Some payment.Shipping.Address.Line2
                            City = payment.Shipping.Address.City
                            State = usState payment.Shipping.Address.State |> Option.get
                            ZipCode = zipCode payment.Shipping.Address.PostalCode |> Option.get
                        }
                    | "CA" ->
                        CanadaStreetAddress {
                            StreetAddress = payment.Shipping.Address.Line1
                            Unit =
                                if System.String.IsNullOrWhiteSpace payment.Shipping.Address.Line2 then
                                    None
                                else
                                    Some payment.Shipping.Address.Line2
                            City = payment.Shipping.Address.City
                            Province = caProvince payment.Shipping.Address.State |> Option.get
                            Postcode = postcode payment.Shipping.Address.PostalCode |> Option.get
                        }
                    | _ ->
                        raise
                        <| new System.ArgumentException(
                            "Unknown country " + payment.Shipping.Address.Country.ToUpperInvariant()
                        )

                let customer: Customer = {
                    order.Customer with
                        Name = {
                            FirstName = nameBits[.. nameBits.Length / 2] |> String.concat " "
                            LastName = nameBits[nameBits.Length / 2 ..] |> String.concat " "
                        }
                        Address = address
                        Email = emailAddress payment.ReceiptEmail |> Option.get
                }

                let transaction: Transaction = { Processed = System.DateTimeOffset.UtcNow; Amount = amount }

                updatePayment orderNumber (CustomerDataUpdated customer) |> ignore
                updatePayment orderNumber (TransactionMade transaction) |> ignore
                updatePayment orderNumber (StatusUpdated PlacedOrder) |> ignore
                new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK)
            | None -> new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.InternalServerError)
        with ex ->
            new APIGatewayHttpApiV2ProxyResponse(
                StatusCode = int HttpStatusCode.BadRequest,
                Body = ex.ToString() + "\r\n" + ex.StackTrace
            )
