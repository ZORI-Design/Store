﻿module Store.UI.Home

open Feliz
open type Feliz.Html
open type Feliz.prop

[<ReactComponent>]
let Home () =
    Html.div [
        prop.style [ style.backgroundColor "#000000"; length.vh 100 |> style.height ]
        prop.text "Home"
    ]
