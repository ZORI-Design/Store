namespace Store.Lambdas.Payments

open System
open Amazon.Lambda.Core
open FSharp.Json
open Store.Shared.DomainModel
open Amazon.Lambda.APIGatewayEvents
open Store.Crm
open System.Net

[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer>)>]
()

type Function() =
    member __.FunctionHandler (request: APIGatewayHttpApiV2ProxyRequest) (_: ILambdaContext) =
        try
            match request.RequestContext.Http.Method.ToUpperInvariant() with
            | "POST" ->
                let payment = Json.deserialize<PaymentPlan> request.Body
                addPayment payment
                new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK, Body = (snd payment.Order).OrderNumber.ToString())
            | "PUT" ->
                match request.QueryStringParameters.TryGetValue "order" with
                | true, orderNum ->
                    match Json.deserialize<PaymentUpdate> request.Body |> updatePayment orderNum with
                    | Some result ->
                        new APIGatewayHttpApiV2ProxyResponse(
                            StatusCode = int HttpStatusCode.OK,
                            Body = Json.serialize result
                        )
                    | None ->
                        raise
                        <| new ArgumentException("Invalid update. Are you sure you supplied the correct order ID?")
                | false, _ -> raise <| new ArgumentException("Missing required parameter order.")
            | method -> raise <| new NotSupportedException(method + " is not a supported method.")
        with ex ->
            new APIGatewayHttpApiV2ProxyResponse(
                StatusCode = int HttpStatusCode.BadRequest,
                Body = ex.ToString() + "\r\n" + ex.StackTrace
            )
