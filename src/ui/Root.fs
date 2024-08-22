module Store.UI.Root

open Feliz
open Feliz.Router
open Store.UI.Error
open Store.UI.Home
open Store.UI.About
open Browser
open Store.UI.Navbar

[<ReactComponent>]
let Root () =
    let (url, setUrl) = React.useState (Router.currentPath ())

    Html.div [
        Navbar url
        React.router [
            router.pathMode
            router.onUrlChanged setUrl
            router.children [
                match url with
                | [] -> Home()
                | [ "about" ] -> About()
                | _ -> Error PageNotFound
            ]
        ]
    ]

let reactRoot = ReactDOM.createRoot (document.getElementById "root")
reactRoot.render (Root())
