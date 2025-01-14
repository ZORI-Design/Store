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
            let cookies =
                match request.Cookies with
                | null -> dict []
                | _ -> request.Cookies |> Array.filter _.Contains('=') |> Array.map (fun (l: string) -> (l.Split('=')[0], l[l.IndexOf('=') + 1 ..])) |> dict

            let correlation =
                match cookies.TryGetValue "correlation" with
                | true, c -> c
                | _ ->
                    [ Random.Shared.NextInt64(); Random.Shared.NextInt64() ]
                    |> List.map uint64
                    |> List.sum
                    |> correlationId

            addInteraction (
                ClientInteraction(
                    PageLoad {
                        PageUrl =
                            sprintf "https://%s%s" request.RequestContext.DomainName request.RequestContext.Http.Path
                        Browser = {
                            CorrelationId = correlation
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
                Headers =
                    dict [
                        ("Location", "https://theleap.co/creator/zorijewelry/")
                        ("Set-Cookie", sprintf "correlation=%s; Max-Age=157680000" correlation) // Max-Age 5yrs
                    ]
            )
        with ex ->
            new APIGatewayHttpApiV2ProxyResponse(
                StatusCode = int HttpStatusCode.BadRequest,
                Body = ex.ToString() + "\r\n" + ex.StackTrace
            )
