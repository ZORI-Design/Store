module Store.UI.Navbar

open Feliz
open Feliz.Router
open type Store.UI.NextUI
open type Feliz.Html
open Fable.Core.JsInterop

[<ReactComponent>]
let DefaultNavbar () =
    let (isMenuOpen, setIsMenuOpen) = React.useState false

    let menuItems = [ "Shop"; "About Us"; "Contact" ]

    Navbar [
        prop.custom ("onMenuOpenChange", setIsMenuOpen)

        prop.style [
            (if isMenuOpen then "#F7F4F0" else "rgba(255,255,255,0)") |> style.backgroundColor
            style.fontFamily "Figtree"
            style.fontWeight 400
            style.float'.left
            length.px 44 |> style.minHeight
            (if isMenuOpen then 0.0 else 0.2) |> style.transitionDelaySeconds
        ]

        prop.children [
            NavbarBrand [
                prop.children [
                    img [
                        prop.src <| import "default" "./assets/Logo.svg"
                        prop.onClick (fun _ -> Router.navigatePath "/")
                        prop.style [
                            length.px 26 |> style.height
                            length.px 26 |> style.minHeight
                            style.cursor "pointer"
                            (if isMenuOpen then 0 else 100) |> style.filter.invert
                        ]
                    ]
                ]
            ]

            NavbarContent [
                prop.className [ "hidden"; "sm:flex"; "gap-4" ]
                prop.style [ length.vw 5 |> style.gap ]
                justify "center"
                prop.children [

                    NavbarItem [
                        Link [
                            prop.text "Shop"
                            prop.onClick (fun _ -> Router.navigatePath "/")
                            prop.style [ style.fontSize 13; style.cursor "pointer"; style.color "white" ]
                        ]
                    ]

                    NavbarItem [
                        Link [
                            prop.text "About Us"
                            prop.onClick (fun _ -> Router.navigatePath "/about")
                            prop.style [ style.fontSize 13; style.cursor "pointer"; style.color "white" ]
                        ]
                    ]

                    NavbarItem [
                        Link [
                            prop.text "Contact"
                            prop.onClick (fun _ -> Router.navigatePath "/contact")
                            prop.style [ style.fontSize 13; style.cursor "pointer"; style.color "white" ]
                        ]
                    ]
                ]
            ]

            NavbarContent [
                justify "end"
                prop.style [ length.px 40 |> style.gap ]
                prop.children [
                    NavbarItem [
                        img [
                            prop.src <| import "default" "./assets/Cart.svg"
                            prop.onClick (fun _ -> Router.navigatePath "/")
                            prop.style [
                                length.px 21 |> style.height
                                style.cursor "pointer"
                                (if isMenuOpen then 0 else 100) |> style.filter.invert
                            ]
                        ]
                    ]

                    NavbarMenuToggle [
                        prop.ariaLabel <| if isMenuOpen then "Close menu" else "Open menu"
                        prop.className "sm:hidden"
                        prop.style [ (if isMenuOpen then "black" else "white") |> style.color ]
                    ]
                ]
            ]

            NavbarMenu [
                prop.style [ style.paddingTop 0; style.backgroundColor "#F7F4F0" ]
                menuItems
                |> List.indexed
                |> List.map (fun (index, item) ->
                    NavbarMenuItem [
                        prop.key $"{item}-{index}"
                        prop.children [
                            Link [
                                color
                                <| if index = 2 then "foreground"
                                   else if index = menuItems.Length - 1 then "danger"
                                   else "foreground"
                                prop.className "w-full"
                                prop.style [
                                    style.fontFamily "the-seasons"
                                    length.perc 320 |> style.fontSize
                                    length.fitContent |> style.height
                                    (if item = "Shop" then 120 else 30) |> style.paddingTop
                                ]
                                prop.text item
                            ]
                        ]

                    ])
                |> prop.children
            ]
        ]
    ]

[<ReactComponent>]
let Navbar =
    function
    | _ -> DefaultNavbar()
