module Store.UI.Product

open Feliz
open Feliz.StripeJs
open Store.Shared.DomainModel
open Store.Shared

open type Feliz.Html
open type Feliz.prop
open type HeroUI
open Fable.Core

[<Measure>]
type g

let stripePromise =
    loadStripe
        "pk_test_51PpM8D091MsxMkVkERmhzCAXb8Jvx0mlNO70qaw2hu428pAVotvH17izg8RWlwgXq5ii2a3HGzolZ8A9unf1aJDZ00QgSNfYmy"

[<ReactComponent>]
let Product (product: Product) (country: Market) =
    let (selectedImage, setSelectedImage) = React.useState 0

    let specRow (header: string) (body: string) =
        tableRow [
            tableHeader [ text header; className "font-bold text-left" ]

            tableCell [ text body; className "text-right" ]
        ]

    let snapChanged : Browser.Types.Event -> unit = function
        | _ ->
            let nodeList = Browser.Dom.document.querySelectorAll "#gallery>*"

            [0..(nodeList.length - 1)]
            |> List.map nodeList.item
            |> List.takeWhile (fun c -> c.getBoundingClientRect().left < 0)
            |> List.length
            |> setSelectedImage

    React.useEffectOnce (fun () ->
        (Browser.Dom.document.getElementById "gallery")
            .addEventListener ("scrollsnapchange", snapChanged))

    div [
        className "flex w-full gap-x-10 gap-y-8 flex-col"

        children [
            div [
                className "w-full flex justify-end flex flex-col gap-y-5"
                children [
                    div [
                        if product.Assets.Length = 0 then
                            [ product.Thumbnail ]
                        else
                            product.Assets
                        |> List.map (fun imgUrl ->
                            img [
                                src imgUrl
                                className "object-cover aspect-4/5 w-full justify-self-center flex-auto snap-center"
                            ])
                        |> children

                        className "flex flex-row overflow-auto snap-x snap-mandatory"
                        style [ style.scrollbarWidth.none ]

                        id "gallery"
                    ]

                    div [
                        className "flex w-full gap-2 justify-center items-center"
                        children [
                            for (idx, url) in product.Assets |> List.indexed do
                                yield
                                    div [
                                        classes [
                                            if selectedImage = idx then "w-1.5" else "w-1"
                                            if selectedImage = idx then "h-1.5" else "h-1"
                                            "rounded-[50%]"
                                        ]
                                        style [
                                            style.backgroundColor (
                                                if selectedImage = idx then
                                                    Colour.indicator.active
                                                else
                                                    Colour.indicator.inactive
                                            )
                                        ]
                                        url.ToString() |> id
                                    ]
                        ]
                    ]
                ]
            ]

            div [
                className "flex w-full flex-col text-center"
                children [
                    h1 [
                        text product.Name
                        className "font-regular text-5xl"
                        style [ style.fontFamily "the-seasons" ]
                    ]

                    p [ text product.Description; style [ style.color Colour.subheader ] ]
                ]
            ]

            div [
                className "flex w-full flex-col px-4"
                children [
                    h2 [ text "Design Concept"; className "font-bold" ]

                    p [ text "Lorum Ipsum Dolor Sit Amet"; className "italic text-sm" ]
                ]
            ]

            table [
                className "mx-4"

                children [
                    tableBody [
                        specRow "Material" "Gold-plated sterling silver"
                        specRow "Weight" "≈ 10.5 g per earring"
                        specRow "Dimensions" "≈ 5.5 cm length"
                    ]
                ]
            ]

            div [
                className "mx-4 p-4 rounded-lg flex flex-col gap-4"
                style [ style.backgroundColor Colour.background ]

                children [
                    div [
                        className "flex flex-row gap-2"

                        children [
                            Html.span [
                                match product.Price with
                                | USD usd -> sprintf "$%.2f" (usd * 1.3m)
                                | CAD cad -> sprintf "$%.2f" (cad * 1.3m)
                                | RMB rmb -> sprintf "¥%.0f" (rmb * 1.3m)
                                |> text

                                className "font-bold line-through"
                            ]

                            Html.span [
                                match product.Price with
                                | USD usd -> sprintf "$%.2f" usd
                                | CAD cad -> sprintf "$%.2f" cad
                                | RMB rmb -> sprintf "¥%.0f" rmb
                                |> text

                                className "font-bold"
                            ]

                            Chip [
                                sprintf "30%% off until %s" ((System.DateTime.Today.AddDays 3).ToString("MMM dd"))
                                |> text
                                custom ("color", "primary")
                                custom ("size", "sm")
                            ]
                        ]
                    ]

                    ul [
                        className "text-sm list-disc ml-4"
                        children [
                            li "90-day free returns, 180-day free exchanges"
                            li "Instant return label provided"
                            li "100% money back guarantee"
                        ]
                    ]

                    Button [ className "rounded-full w-full"; text "Buy Now"; custom ("color", "primary") ]
                ]
            ]
        ]
    ]
