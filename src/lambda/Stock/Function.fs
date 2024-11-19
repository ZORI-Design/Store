namespace Store.Lambdas.Stock

open System
open Amazon.Lambda.Core
open Thoth.Json.Net
open Store.Shared.DomainModel
open Amazon.Lambda.APIGatewayEvents
open Store.Crm
open System.Net
open Amazon.DynamoDBv2.DocumentModel

[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer>)>]
()

type Function() =
    member __.FunctionHandler (request: APIGatewayHttpApiV2ProxyRequest) (_: ILambdaContext) =
        try
            let pathParam =
                match request.PathParameters.TryGetValue "product" with
                | true, v -> Some v
                | false, _ -> None

            match request.RequestContext.Http.Method.ToUpperInvariant(), pathParam with
            | "GET", None ->
                stock ()
                |> Seq.toArray
                |> fun s -> Encode.Auto.toString (s, extra = (Extra.empty |> Extra.withDecimal))
                |> fun b -> new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK, Body = b)
            | "GET", Some c when c.Equals("catalogue", StringComparison.InvariantCultureIgnoreCase) ->
                catalogue ()
                |> Seq.toArray
                |> fun s -> Encode.Auto.toString (s, extra = (Extra.empty |> Extra.withDecimal))
                |> fun b -> new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK, Body = b)
            | "GET", Some product ->
                genericQuery<Product * Quantity> "Stock" (new QueryFilter("key", QueryOperator.Equal, product))
                |> Seq.toArray
                |> fun s -> Encode.Auto.toString (s, extra = (Extra.empty |> Extra.withDecimal))
                |> fun b -> new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK, Body = b)
            | "PUT", Some path ->
                match request.QueryStringParameters.TryGetValue "q" with
                | true, i -> Some i
                | false, _ -> None
                |> Option.bind (fun s ->
                    match Int32.TryParse s with
                    | true, i -> Some i
                    | false, _ -> None)
                |> Option.bind quantity
                |> Option.bind (fun quantity ->
                    genericQuery<Product * Quantity> "Stock" (new QueryFilter("key", QueryOperator.Equal, path))
                    |> Seq.tryHead
                    |> Option.map (fun h -> (h, quantity)))
                |> Option.map (fun ((product, _), quantity) ->
                    match updateStockQuantity product quantity with
                    | Ok _ -> new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK)
                    | Error e ->
                        new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.BadRequest, Body = e))
                |> Option.defaultValue (
                    new APIGatewayHttpApiV2ProxyResponse(
                        StatusCode = int HttpStatusCode.BadRequest,
                        Body = "Invalid request. Is your quantity valid?"
                    )
                )
            | method, _ -> raise <| new NotSupportedException(method + " is not a supported method.")
        with ex ->
            new APIGatewayHttpApiV2ProxyResponse(
                StatusCode = int HttpStatusCode.BadRequest,
                Body = ex.ToString() + "\r\n" + ex.StackTrace
            )
