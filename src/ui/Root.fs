module Store.UI.Root

open Feliz
open Feliz.Router
open Store.UI.Error
open Store.UI.Home
open Browser

[<ReactComponent>]
let Root () =
    let (url, setUrl) = React.useState (Router.currentPath ())

    React.router [
        router.pathMode
        router.onUrlChanged setUrl
        router.children [
            match url with
            | [ ] -> Home ()
            | ["about"] -> Html.h1 "About Page"
            | _ -> Error PageNotFound
        ]
    ]

let reactRoot = ReactDOM.createRoot (document.getElementById "root")
reactRoot.render (Root())
