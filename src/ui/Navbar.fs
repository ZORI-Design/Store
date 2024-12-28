module Store.UI.Navbar

open Feliz
open Feliz.Router
open type Store.UI.NextUI
open type Feliz.Html
open Fable.Core.JsInterop

[<ReactComponent>]
let DefaultNavbar (solidBackground: bool) =
    let (isMenuOpen, setIsMenuOpen) = React.useState false

    let menuItems = [ "Shop"; "About Us"; "Contact" ]

    Navbar [
        prop.custom ("onMenuOpenChange", setIsMenuOpen)

        prop.style [
            (if isMenuOpen then "#F2F0ED"
             elif solidBackground then "#999999"
             else "rgba(255, 255, 255, 0)")
            |> style.backgroundColor
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
                        prop.src <| import "default" "./assets/logo.svg"
                        prop.onClick (fun _ -> Router.navigatePath "/")
                        prop.style [
                            length.px 26 |> style.height
                            style.cursor "pointer"
                            (if isMenuOpen then 0 else 100) |> style.filter.invert
                            style.transitionDurationSeconds 0.3
                        ]
                    ]
                ]
            ]

            NavbarContent [
                prop.className [ "hidden"; "sm:flex"; "gap-4" ]
                prop.style [ length.px 72 |> style.gap ]
                justify "center"
                prop.children [

                    NavbarItem [
                        Link [
                            prop.className [ "text-small" ]
                            prop.text "Shop"
                            prop.onClick (fun _ -> Router.navigatePath "/")
                            prop.style [ style.cursor "pointer"; style.color "white" ]
                        ]
                    ]

                    NavbarItem [
                        Link [
                            prop.className [ "text-small" ]
                            prop.text "About Us"
                            prop.onClick (fun _ -> Router.navigatePath "/about")
                            prop.style [ style.cursor "pointer"; style.color "white" ]
                        ]
                    ]

                    NavbarItem [
                        Link [
                            prop.className [ "text-small" ]
                            prop.text "Contact"
                            prop.onClick (fun _ -> Router.navigatePath "/contact")
                            prop.style [ style.cursor "pointer"; style.color "white" ]
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
                            prop.src <| import "default" "./assets/cart.svg"
                            prop.onClick (fun _ -> Router.navigatePath "/cart")
                            prop.style [
                                length.px 21 |> style.height
                                style.cursor "pointer"
                                (if isMenuOpen then 0 else 100) |> style.filter.invert
                                style.transitionDurationSeconds 0.3
                            ]
                        ]
                    ]

                    NavbarMenuToggle [
                        prop.ariaLabel <| if isMenuOpen then "Close menu" else "Open menu"
                        prop.className "sm:hidden"
                        prop.style [
                            (if isMenuOpen then "black" else "white") |> style.color
                            style.transitionDurationSeconds 0.3
                        ]
                    ]
                ]
            ]

            NavbarMenu [
                prop.style [ style.paddingTop 0; style.backgroundColor "#F2F0ED" ]
                menuItems
                |> List.indexed
                |> List.map (fun (index, item) ->
                    NavbarMenuItem [
                        prop.key $"{item}-{index}"
                        prop.children [
                            Link [
                                color "foreground"
                                prop.className "w-full"
                                prop.style [
                                    style.fontFamily "the-seasons"
                                    length.fitContent |> style.height
                                    (if index = 0 then 120 else 40) |> style.paddingTop
                                ]
                                prop.className [ "text-large" ]
                                prop.text item
                            ]
                        ]

                    ])
                |> prop.children
            ]
        ]
    ]

[<ReactComponent>]
let DeadNavbar () =
    div [
        prop.style [ style.backgroundColor "transparent" ]

        prop.classes [ "flex"; "justify-center"; "align-center"; "py-3"; "h-12" ]

        prop.children [
            img [
                prop.src <| import "default" "./assets/logo.svg"
                prop.style [ length.px 22 |> style.height ]
            ]
        ]
    ]

[<ReactComponent>]
let Navbar =
    function
    | [ "store-policies" ] -> DeadNavbar()
    | [] -> DefaultNavbar false
    | _ -> DefaultNavbar true
