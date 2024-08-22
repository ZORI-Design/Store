module Store.UI.Navbar

open Feliz
open Feliz.Router
open type Feliz.Html

[<ReactComponent>]
let Navbar pageRoute =
    ul [
        li [
            a [
                prop.text "About"
                prop.onClick (fun _ -> Router.navigatePath "/about")
                prop.style [
                    style.cursor "pointer"
                ]
            ]
        ]
    ]