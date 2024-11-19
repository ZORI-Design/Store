module Store.UI.Root

open System

open Browser
open Browser.Navigator
open Feliz
open Feliz.Router
open Store.Shared.DomainModel
open Store.UI.About
open Store.UI.Error
open Store.UI.Home
open Store.UI.Product
open Store.UI.Navbar
open Store.UI.Api
open Fable.Core.JsInterop

[<ReactComponent>]
let Root () =
    let (url, setUrl) = React.useState (Router.currentPath ())

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
                        | [ "button" ] -> NextUI.Button [ prop.text "Hello!"; prop.custom ("color", "primary") ]
                        | [ "product"; productName ] ->
                            let (catalogue, setCatalogue) = React.useState<Catalogue> []

                            React.useEffectOnce (fun () ->
                                getCatalogue (fun c ->
                                    printfn "%A" c
                                    setCatalogue c))

                            match
                                catalogue
                                |> Seq.tryFind (fun ci ->
                                    ci.Name.Equals(productName, StringComparison.InvariantCultureIgnoreCase))
                            with
                            | Some product -> Product product
                            | None -> Error PageNotFound
                        | _ -> Error PageNotFound
                    ]
                ]
            ]
            |> Interop.reactApi.Children.toArray
    |}

let reactRoot = ReactDOM.createRoot (document.getElementById "root")
reactRoot.render (Root())
