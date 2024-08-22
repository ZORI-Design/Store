module Store.UI.Navbar

open Feliz
open Feliz.Router
open type Store.UI.NextUI
open type Feliz.Html

[<ReactComponent>]
let Navbar pageRoute =
    Navbar [
        NavbarBrand [
            prop.children [
                p [
                    prop.text "ZORI"
                    prop.className [ "font-bold"; "text-inherit" ]
                ]
            ]
        ]

        NavbarContent [
            prop.className [ "sm:flex"; "gap-4" ]
            prop.custom ("justify", "center")
            prop.children [
                NavbarItem [
                    Link [
                        color "foreground"
                        prop.href "#"
                        prop.text "Products"
                    ]
                ]

                NavbarItem [
                    prop.custom ("isActive", ())
                    prop.children [
                        Link [
                            prop.href "#"
                            prop.custom ("aria-current", "page")
                            prop.text "About Us"
                        ]
                    ]
                ]
                
                NavbarItem [
                    Link [
                        color "foreground"
                        prop.href "#"
                        prop.text "Contact"
                    ]
                ]
            ]
        ]

        NavbarContent [
            prop.custom ("justify", "end")
            prop.children [
                NavbarItem [
                    Button [
                        color "primary"
                        prop.href "#"
                        prop.custom ("variant", "flat")
                        prop.text "Cart"
                    ]
                ]
            ]
        ]
    ]
