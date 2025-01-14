namespace Store.Lambdas.Interactions


open Amazon.Lambda.Core
open Thoth.Json.Net
open Store.Crm
open Store.Shared.DomainModel
open Amazon.Lambda.APIGatewayEvents
open System.Net
open System


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer>)>]
()


type Function() =
    member __.FunctionHandler (request: APIGatewayHttpApiV2ProxyRequest) (_: ILambdaContext) =
        try
            let cookies =
                request.Cookies |> Array.map (fun (l: string) -> (l.Split('=')[0], l[l.IndexOf('=') + 1 ..])) |> dict

            match Decode.Auto.fromString<Interaction> request.Body with
            | Ok v ->
                let correlation =
                    match cookies.TryGetValue "correlation" with
                    | true, c -> c
                    | _ ->
                        [ Random.Shared.NextInt64(); Random.Shared.NextInt64() ]
                        |> List.map uint64
                        |> List.sum
                        |> correlationId

                addInteraction v
                new APIGatewayHttpApiV2ProxyResponse(StatusCode = int HttpStatusCode.OK, Headers = dict [("Set-Cookie", sprintf "correlation=%s" correlation)])
            | Error e -> failwith e

        with ex ->
            new APIGatewayHttpApiV2ProxyResponse(
                StatusCode = int HttpStatusCode.BadRequest,
                Body = ex.ToString() + "\r\n" + ex.StackTrace
            )
