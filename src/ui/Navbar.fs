module Store.UI.Navbar

open Feliz
open Feliz.Router
open type Store.UI.NextUI
open type Feliz.Html

[<ReactComponent>]
let DefaultNavbar () =
    Navbar [
        prop.style [
            style.backgroundColor "#ffffff"
        ]

        prop.children [
            NavbarBrand [
                prop.children [
                    p [
                        prop.text "ZORI"
                        prop.className [ "font-bold"; "text-inherit" ]
                        prop.onClick (fun _ -> Router.navigatePath "/")
                        prop.style [
                            style.cursor "pointer"
                        ]
                    ]
                ]
            ]

            NavbarContent [
                prop.className [ "sm:flex"; "gap-4" ]
                prop.custom ("justify", "center")
                prop.children [
                    NavbarItem [
                        Link [
                            prop.text "Products"
                            color "foreground"
                            prop.onClick (fun _ -> Router.navigatePath "/products")
                            prop.style [
                                style.cursor "pointer"
                            ]
                        ]
                    ]

                    NavbarItem [
                        prop.custom ("isActive", ())
                        prop.children [
                            Link [
                                prop.text "About Us"
                                prop.onClick (fun _ -> Router.navigatePath "/about")
                                prop.style [
                                style.cursor "pointer"
                            ]
                            ]
                        ]
                    ]
                
                    NavbarItem [
                        Link [
                            prop.text "Contact"
                            color "foreground"
                            prop.onClick (fun _ -> Router.navigatePath "/contact")
                            prop.style [
                                style.cursor "pointer"
                            ]
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
    ]

[<ReactComponent>]
let Navbar = function
| _ -> DefaultNavbar()