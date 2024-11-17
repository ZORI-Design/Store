module Store.UI.Product

open Feliz
open Store.Shared.DomainModel

open type Feliz.Html
open type Feliz.prop
open type NextUI

[<ReactComponent>]
let Product (product: Product) =
    div [
        className "flex w-full gap-x-4"
        children [
            div [
                className "flex flex-col w-1/12 gap-y-4"
                children [
                    Image [
                        src "https://nextui.org/images/hero-card-complete.jpeg"
                        className "object-cover aspect-4/5"
                    ]
                    Image [
                        src "https://nextui.org/images/hero-card-complete.jpeg"
                        className "object-cover aspect-4/5"
                    ]
                    Image [
                        src "https://nextui.org/images/hero-card-complete.jpeg"
                        className "object-cover aspect-4/5"
                    ]
                ]
            ]

            div [
                className "w-1/2"
                children [
                    Image [
                        src "https://nextui.org/images/hero-card-complete.jpeg"
                        className "object-cover aspect-4/5 w-full justify-self-center"
                    ]
                ]
            ]

            div [
                className "w-5/12 flex flex-col gap-y-8"
                children [
                    h1 [ text product.Name; className "text-5xl" ]
                    p [
                        children [
                            match product.Price with
                            | USD usd -> sprintf "US$%0.2f" usd
                            | CAD cad -> sprintf "C$%0.2f" cad
                            | RMB rmb -> sprintf "¥%0.2f" rmb
                            |> Html.text
                            br []
                            Html.text "Return at 0 cost for 17 days "
                            Link [ text "Read our return policy"; custom ("underline", "always") ]
                        ]
                    ]
                    Accordion [
                        AccordionItem [
                            className "w-1/2"
                            custom (
                                "title",
                                p [
                                    text "One sentence about the product story."
                                    className "font-semibold text-base"
                                ]
                            )
                            custom ("subtitle", "Read the Story")
                            children [ Html.text "Example!" ]
                        ]
                    ]

                    ul [
                        li [ sprintf "Material: %A" product.Plating |> text ]
                        li [ sprintf "Weight: %dmg per pair" product.Mass |> text ]
                        li [ text "Dimensions: ?" ]
                    ]

                    Button [ text "Button"; className "w-1/2" ]
                    Button [ text "Button"; className "w-1/2" ]
                    Button [ text "Button"; className "w-1/2" ]
                ]
            ]
        ]
    ]
