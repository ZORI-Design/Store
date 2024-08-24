module Store.UI.Navbar

open Feliz
open Feliz.Router
open type Store.UI.NextUI
open type Feliz.Html
open Fable.Core.JsInterop

[<ReactComponent>]
let DefaultNavbar () =
    let (isMenuOpen, setIsMenuOpen) = React.useState false

    let menuItems = [
        "Products";
        "About ZORI";
        "Contact Us"
    ]

    Navbar [
        prop.custom ("onMenuOpenChange", setIsMenuOpen)
        prop.style [
            style.backgroundColor "#ffffff00
            "
            style.fontFamily "Figtree"
            style.fontWeight 400
            length.vw 8 |> style.height
            length.px 44 |> style.minHeight
            length.px 48 |> style.maxHeight
        ]

        prop.children [
            NavbarBrand [
                prop.children [
                    img [
                        prop.src <| import "default" "./assets/Logo.svg"
                        prop.onClick (fun _ -> Router.navigatePath "/")
                        prop.style [
                            length.px 24 |> style.height
                            length.px 24 |> style.minHeight
                            style.cursor "pointer"
                        ]
                    ]
                ]
            ]

            NavbarContent [
                prop.className [ "hidden"; "sm:flex"; "gap-4" ]
                prop.style [ length.vw 5 |> style.gap ]
                prop.custom ("justify", "center")
                prop.children [

                    NavbarItem [
                        Link [
                            prop.text "Products"
                            color "foreground"
                            prop.onClick (fun _ -> Router.navigatePath "/")
                            prop.style [ style.fontSize 13; style.cursor "pointer" ]
                        ]
                    ]

                    NavbarItem [
                        Link [
                            prop.text "About ZORI"
                            color "foreground"
                            prop.onClick (fun _ -> Router.navigatePath "/about")
                            prop.style [ style.fontSize 13; style.cursor "pointer" ]
                        ]
                    ]

                    NavbarItem [
                        Link [
                            prop.text "Contact Us"
                            color "foreground"
                            prop.onClick (fun _ -> Router.navigatePath "/contact")
                            prop.style [ style.fontSize 13; style.cursor "pointer" ]
                        ]
                    ]
                ]
            ]

            NavbarContent [
                prop.custom ("justify", "end")
                prop.style [ length.px 32 |> style.gap ]
                prop.children [
                    NavbarItem [
                        img [
                            prop.src <| import "default" "./assets/Cart.svg"
                            prop.onClick (fun _ -> Router.navigatePath "/")
                            prop.style [
                                length.px 21 |> style.height
                                length.px 21 |> style.minHeight
                                style.cursor "pointer"
                            ]
                        ]
                    ]
                    
                    NavbarMenuToggle [
                        prop.ariaLabel <| if isMenuOpen then "Close menu" else "Open menu"
                        prop.className "sm:hidden"
                    ]
                ]
            ]

            NavbarMenu [
            ]
        ]
    ]

[<ReactComponent>]
let Navbar =
    function
    | _ -> DefaultNavbar()
