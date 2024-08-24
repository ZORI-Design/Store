module Store.UI.Error

open Feliz
open type Feliz.Html
open Browser.Dom

type ErrorCode =
    | PageNotFound
    | InternalError

[<ReactComponent>]
let Error =
    function
    | PageNotFound ->
        React.useEffectOnce (fun () -> document.title <- "404 - Page Not Found")
        div [ h1 "Error - Page not found"; p "The requested page could not be found." ]
    | InternalError ->
        React.useEffectOnce (fun () -> document.title <- "500 - Internal Error")

        div [
            h1 "Internal Error"
            p "The server encountered an error while processing your request."
        ]
