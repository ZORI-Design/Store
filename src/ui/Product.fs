module Store.UI.Product

open Feliz
open Store.Shared.DomainModel

open type Feliz.Html
open type Feliz.prop
open type NextUI

[<Measure>]
type g

[<ReactComponent>]
let Product (product: Product) =
    let (selectedImage, setSelectedImage) = React.useState 0
    div [
        className "flex w-full gap-x-4"
        children [
            div [
                className "flex flex-col w-1/12 gap-y-4"
                children [
                    for (idx, a) in product.Assets |> List.indexed do
                        yield Image [
                            src a
                            className "object-cover aspect-1/1"
                            onClick (fun _ -> setSelectedImage idx)
                            id idx
                        ]
                ]
            ]

            div [
                className "w-1/2"
                children [
                    Image [
                        src (if product.Assets.IsEmpty then product.Thumbnail else product.Assets[selectedImage])
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
                            | RMB rmb -> sprintf "�%0.2f" rmb
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
                        li [ sprintf "Weight: %.1f g per pair" ((float product.Mass) * 1.0<mg> / 1000.0<mg/g>) |> text ]
                        li [ text "Dimensions: ?" ]
                    ]

                    Button [ text "Button"; className "w-1/2" ]
                    Button [ text "Button"; className "w-1/2" ]
                    Button [ text "Button"; className "w-1/2" ]
                ]
            ]
        ]
    ]
