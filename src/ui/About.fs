module Store.UI.About

open Feliz
open type Feliz.Html
open type Feliz.prop

open Store.Shared

[<ReactComponent>]
let About () =
    div [
        classes [ "w-screen"; "h-screen"; "overflow-hidden" ]

        style [ style.backgroundColor Colour.background ]

        children [ h1 "About ZORI" ]
    ]
