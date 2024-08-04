module Store.UI.Root

open System
open Feliz
open type Html
open Browser

[<ReactComponent>]
let Root () =
    let name, setName = React.useState "Zori"

    let title =
        if String.IsNullOrWhiteSpace(name) then
            "Nobody"
        else
            name.ToUpperInvariant()
        |> sprintf "%s's Site"

    let paddedName =
        if String.IsNullOrWhiteSpace name then
            ""
        else
            sprintf " %s" name

    React.useEffect ((fun () -> document.title <- title), [| box name |])

    div [
        h1 $"Hi{paddedName}! Welcome to your website!"
        span "Name: "
        input [ prop.value name; prop.onChange setName ]
    ]

let reactRoot = ReactDOM.createRoot (document.getElementById "root")
reactRoot.render (Root())
