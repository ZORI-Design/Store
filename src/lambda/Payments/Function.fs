namespace Store.Lambdas.Payments

open Amazon.Lambda.Core
open FSharp.Data
open Amazon.Lambda.APIGatewayEvents
open Store.Shared.DomainModel
open Store.Crm
open System.Net


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer>)>]
()

type PaymentIntent = JsonProvider<"""{"id":"pi_3MtwBwLkdIwHu7ix28a3tqPa","object":"payment_intent","amount":2000,"amount_capturable":0,"amount_details":{"tip":{}},"amount_received":0,"application":null,"application_fee_amount":null,"automatic_payment_methods":{"enabled":true},"canceled_at":null,"cancellation_reason":null,"capture_method":"automatic","client_secret":"pi_3MtwBwLkdIwHu7ix28a3tqPa_secret_YrKJUKribcBjcG8HVhfZluoGH","confirmation_method":"automatic","created":1680800504,"currency":"usd","customer":null,"description":null,"invoice":null,"last_payment_error":null,"latest_charge":null,"livemode":false,"metadata":{"correlation":"abc abc"},"next_action":null,"on_behalf_of":null,"payment_method":null,"payment_method_options":{"card":{"installments":null,"mandate_options":null,"network":null,"request_three_d_secure":"automatic"},"link":{"persistent_token":null}},"payment_method_types":["card","link"],"processing":null,"receipt_email":"abc@abc.com","review":null,
"setup_future_usage":null,"shipping":{"address":{"city":"abc abc","country":"CA","line1":"abc abc","line2":"abc abc","postal_code":"abc abc","state":"BC"},"carrier":"abc abc","name":"abc abc","phone":"abc abc","tracking_number":null},"source":null,"statement_descriptor":null,"statement_descriptor_suffix":null,"status":"requires_payment_method","transfer_data":null,"transfer_group":null}""">

type Function() =
    member __.FunctionHandler (request: APIGatewayHttpApiV2ProxyRequest) (_: ILambdaContext) =
        try
            let payment = PaymentIntent.Load request.Body
            let amount =
                match payment.Currency.ToUpperInvariant() with
                | "USD" -> USD
                | "CAD" -> CAD
                | "RMB" -> RMB
                | _ -> raise <| new System.NotSupportedException("Unsupported currency " + payment.Currency.ToUpperInvariant())
                <| (decimal payment.Amount / 100m)

            let nameBits = payment.Shipping.Name.Split()
            let correlation : CorrelationId = correlationId(int payment.Metadata.Correlation[..payment.Metadata.Correlation.Length / 2], int payment.Metadata.Correlation[payment.Metadata.Correlation.Length / 2..])

            let address =
                match payment.Shipping.Address.Country.ToUpperInvariant() with
                | "US" -> UsStreetAddress {
                                StreetAddress = payment.Shipping.Address.Line1
                                Unit = if System.String.IsNullOrWhiteSpace payment.Shipping.Address.Line2 then None else Some payment.Shipping.Address.Line2
                                City = payment.Shipping.Address.City
                                State = usState payment.Shipping.Address.State |> Option.get
                                ZipCode = zipCode payment.Shipping.Address.PostalCode |> Option.get
                            }
                | "CA" -> CanadaStreetAddress {
                                StreetAddress = payment.Shipping.Address.Line1
                                Unit = if System.String.IsNullOrWhiteSpace payment.Shipping.Address.Line2 then None else Some payment.Shipping.Address.Line2
                                City = payment.Shipping.Address.City
                                Province = caProvince payment.Shipping.Address.State |> Option.get
                                Postcode = postcode payment.Shipping.Address.PostalCode |> Option.get
                            }
                | _ -> raise <| new System.ArgumentException("Unknown country " + payment.Shipping.Address.Country.ToUpperInvariant())

            let customer : Customer = {
                Name = {
                    FirstName = nameBits[..nameBits.Length / 2] |> String.concat " "
                    LastName = nameBits[nameBits.Length / 2..] |> String.concat " "
                }
                CorrelationId = correlation
                Address = address
                Email = emailAddress payment.ReceiptEmail |> Option.get
                Phone = None
                OrderHistory = []
            }

            let orderData : OrderData = {
                Items = []
                OrderNumber = 0u
                TrackingNumber = None
            }

            let transaction : Transaction = {
                Processed = System.DateTimeOffset.UtcNow
                Amount = amount
            }

            let plan : PaymentPlan = {
                Customer = tryFindCustomer correlation |> Option.defaultValue customer
                Order = ([ PlacedOrder, System.DateTimeOffset.UtcNow ], orderData)
                Balance = amount
                RemainingInstallments = 0u
                TotalInstallments = 1u
                Transactions = [ transaction ]
            }

            addPayment plan

            new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK)
        with ex ->
            new APIGatewayHttpApiV2ProxyResponse(
                StatusCode = int HttpStatusCode.BadRequest,
                Body = ex.ToString() + "\r\n" + ex.StackTrace
            )
