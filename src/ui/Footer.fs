module Store.UI.Footer

open Feliz
open type Feliz.Html
open type Feliz.prop
open type Store.UI.Icon

[<ReactComponent>]
let Footer() = div [
    classes [
        "w-screen"
        "flex"
        "justify-between"
        "px-8"
        "py-6"
        "text-xs"
    ]
    
    children [
        Html.text "© 2025 ZORI Design"
        Instagram
    ]
]