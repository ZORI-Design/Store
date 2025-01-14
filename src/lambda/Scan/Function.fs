namespace Store.Lambdas.Scan

open Amazon.Lambda.Core
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
            addInteraction (
                ClientInteraction(
                    PageLoad {
                        PageUrl =
                            sprintf "https://%s%s" request.RequestContext.DomainName request.RequestContext.Http.Path
                        Browser = {
                            CorrelationId = correlationId <| uint64 0
                            Browser = request.RequestContext.Http.UserAgent
                            FormFactor =
                                match request.Headers.TryGetValue "sec-ch-ua-mobile" with
                                | true, "?0" -> DesktopBrowser
                                | _ -> MobileBrowser
                            ScreenRes = (0u, 0u)
                            Languages =
                                match request.Headers.TryGetValue "accept-language" with
                                | true, ls -> ls.Split(",")
                                | _ -> [| "en" |]
                                |> Array.toList
                                |> List.map _.Split(';').[0]
                        }
                    },
                    DateTimeOffset.UtcNow
                )
            )

            new APIGatewayHttpApiV2ProxyResponse(
                StatusCode = int HttpStatusCode.TemporaryRedirect,
                Headers = dict [ ("Location", "https://theleap.co/creator/zorijewelry/") ]
            )
        with ex ->
            new APIGatewayHttpApiV2ProxyResponse(
                StatusCode = int HttpStatusCode.BadRequest,
                Body = ex.ToString() + "\r\n" + ex.StackTrace
            )
