namespace Store.Lambdas.Interactions


open Amazon.Lambda.Core
open Thoth.Json.Net
open Store.Crm
open Store.Shared.DomainModel
open Amazon.Lambda.APIGatewayEvents
open System.Net


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer>)>]
()


type Function() =
    member __.FunctionHandler (request: APIGatewayHttpApiV2ProxyRequest) (_: ILambdaContext) =
        try
            match Decode.Auto.fromString<Interaction> request.Body with
            | Ok v -> addInteraction v
            | Error e -> failwith e

            new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK)
        with ex ->
            new APIGatewayHttpApiV2ProxyResponse(
                StatusCode = int HttpStatusCode.BadRequest,
                Body = ex.ToString() + "\r\n" + ex.StackTrace
            )
