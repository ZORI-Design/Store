namespace Store.Lambdas.Leads

open Amazon.Lambda.Core
open FSharp.Json
open Amazon.Lambda.APIGatewayEvents
open Store.Shared.DomainModel
open Store.Crm
open System.Net


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer>)>]
()


type Function() =
    member __.FunctionHandler (request: APIGatewayHttpApiV2ProxyRequest) (_: ILambdaContext) =
        try
            Json.deserialize<Lead> request.Body |> addLead

            new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK)
        with ex ->
            new APIGatewayHttpApiV2ProxyResponse(
                StatusCode = int HttpStatusCode.BadRequest,
                Body = ex.ToString() + "\r\n" + ex.StackTrace
            )
