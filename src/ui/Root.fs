module Store.UI.Root

open System

open Browser
open Feliz
open Feliz.Router
open Store.Shared
open Store.Shared.DomainModel
open Store.UI.About
open Store.UI.Policy
open Store.UI.Error
open Store.UI.Home
open Store.UI.Product
open Store.UI.Navbar
open Store.UI.Api
open Fable.Core.JsInterop

[<ReactComponent>]
let Root () =
    let (url, setUrl) = React.useState (Router.currentPath ())
    let (country, setCountry) = React.useState Canada

    React.useEffect (
        (fun () ->
            let d: BrowserData = {
                CorrelationId = correlationId <| uint64 0
                Browser = navigator.userAgent
                FormFactor =
                    if (emitJsExpr () "(navigator.userAgentData?.mobile ?? false)") then
                        MobileBrowser
                    else
                        DesktopBrowser
                ScreenRes = (window.screen.width |> uint, window.screen.height |> uint)
                Languages = navigator.languages.Value |> Array.toList
            }

            logLoad ("http://zorijewelry.com/" + String.concat "/" url, d) |> ignore),
        [| box url |]
    )

    React.useEffectOnce (
        (fun () -> 
            promise {
                let! resp = Fetch.fetch "https://api.country.is/" []
                let! json = resp.json()
                let value = json :?> {| ip: string; country: string |}
                match value.country with
                | "CA" -> Canada
                | "US" | _ -> USA
                |> setCountry
            }
            |> Promise.start
        )
    )

    NextUI.NextUIProvider {|
        navigate = Router.navigatePath
        children =
            [
                Navbar url
                React.router [
                    router.pathMode
                    router.onUrlChanged setUrl
                    router.children [
                        match url with
                        | [] -> Home()
                        | [ "about" ] -> About()
                        | [ "store-policies" ] -> Policy()
                        | [ "product"; productName ] ->
                            let (catalogue, setCatalogue) = React.useState<Catalogue> []

                            React.useEffectOnce (fun () -> getCatalogue setCatalogue)

                            match
                                catalogue
                                |> Seq.tryFind (fun ci ->
                                    ci.Name.Equals(productName, StringComparison.InvariantCultureIgnoreCase))
                            with
                            | Some product -> Product product country
                            | None when Seq.isEmpty catalogue ->
                                Html.div [
                                    prop.style [ style.backgroundColor Colour.background ]
                                    prop.classes [
                                        "top-0"
                                        "left-0"
                                        "right-0"
                                        "bottom-0"
                                        "absolute"
                                        "flex"
                                        "items-center"
                                        "justify-center"
                                    ]

                                    prop.children [ ReactSpinners.ClipLoader [ prop.custom ("loading", true) ] ]
                                ]
                            | None -> Error PageNotFound
                        | _ -> Error PageNotFound
                    ]
                ]
            ]
            |> Interop.reactApi.Children.toArray
    |}

let reactRoot = ReactDOM.createRoot (document.getElementById "root")
reactRoot.render (Root())
