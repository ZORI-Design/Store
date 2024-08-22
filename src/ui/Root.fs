module Store.UI.Root

open Feliz
open Feliz.Router
open Store.UI.Error
open Store.UI.Home
open Store.UI.About
open Browser

[<ReactComponent>]
let Root () =
    let (url, setUrl) = React.useState (Router.currentPath ())

    NextUI.NextUIProvider {|
        navigate = Router.navigatePath
        children =
            [
                React.router [
                    router.pathMode
                    router.onUrlChanged setUrl
                    router.children [
                        match url with
                        | [] -> Home()
                        | [ "about" ] -> About()
                        | [ "button" ] -> NextUI.Button [ prop.text "Hello!"; prop.custom ("color", "primary") ]
                        | _ -> Error PageNotFound
                    ]
                ]
            ]
            |> Interop.reactApi.Children.toArray
    |}

let reactRoot = ReactDOM.createRoot (document.getElementById "root")
reactRoot.render (Root())
