namespace Store.Lambdas.Stock

open System
open Amazon.Lambda.Core
open FSharp.Json
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
            let fullPath = request.RawPath.Trim('/')
            // Trim it down to just the product name.
            let path = fullPath[fullPath.IndexOf('/') ..].Trim('/')

            match request.RequestContext.Http.Method.ToUpperInvariant() with
            | "GET" when path |> String.IsNullOrEmpty ->
                catalogue ()
                |> Json.serialize
                |> fun b -> new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK, Body = b)
            | "GET" ->
                genericQuery<Product * Quantity> "Stock" (new QueryFilter("key", QueryOperator.Equal, path))
                |> Seq.toArray
                |> Json.serialize
                |> fun b -> new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK, Body = b)
            | "PUT" ->
                match request.PathParameters.TryGetValue "q" with
                | true, v -> Some v
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
            | method -> raise <| new NotSupportedException(method + " is not a supported method.")
        with ex ->
            new APIGatewayHttpApiV2ProxyResponse(
                StatusCode = int HttpStatusCode.BadRequest,
                Body = ex.ToString() + "\r\n" + ex.StackTrace
            )
