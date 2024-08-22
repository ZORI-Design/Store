module Store.UI.Home

open Feliz
open type Feliz.Html
open type Feliz.prop

[<ReactComponent>]
let Home () =
    h1 [ text "Home"; className [ "text-3xl"; "font-bold"; "underline" ] ]
