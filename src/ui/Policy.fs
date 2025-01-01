module Store.UI.Policy

open Browser
open Feliz
open Store.Shared
open Store.UI.Footer

open type Feliz.Html
open type Feliz.prop
open type NextUI

[<ReactComponent>]
let Policy () =
    let (selected, setSelected) = React.useState<string option> None

    let accordionIndicator =
        custom (
            "indicator",
            Svg.svg [
                svg.classes [ "transition-transform"; "-rotate-90"; "group-data-[open=true]:rotate-180" ]
                svg.custom ("aria-hidden", true)
                svg.fill "none"
                svg.custom ("focusable", false)
                svg.custom ("height", length.rem 1.85)
                svg.custom ("role", "presentation")
                svg.viewBox (0, 0, 24, 24)
                svg.custom ("width", length.rem 1.85)

                svg.children [
                    Svg.path [
                        svg.d "M15.5 19l-7-7 7-7"
                        svg.stroke Colour.dropdown
                        svg.strokeLineCap "round"
                        svg.strokeLineJoin "round"
                        svg.strokeWidth 1.5
                    ]
                ]
            ]
        )

    let accordionTitle (name: string) tag =
        p (
            match selected with
            | Some t when t = tag -> [
                text name
                classes [ "text-xl"; "font-bold" ]
                style [ style.color Colour.header ]
              ]
            | _ -> [
                text name
                classes [ "text-xl"; "font-medium" ]
                style [ style.color Colour.paragraph ]
              ]
        )

    let accordionItem (name: string) (tag: string) (content: Fable.React.ReactElement seq) =
        AccordionItem [
            classes [ "group"; "px-5"; "py-1" ]

            custom ("title", accordionTitle name tag)

            accordionIndicator

            children content
        ]

    div [
        style [ style.backgroundColor Colour.background ]

        classes [ "w-screen"; "min-h-screen"; "pt-24"; "flex"; "flex-col"; "items-center" ]

        children [
            div [
                children [
                    a [
                        text "← Back to Store"
                        href "https://theleap.co/@zorijewelry"
                        classes [ "font-medium" ]
                    ]
                ]

                classes [ "sticky"; "top-12"; "w-screen"; "text-center"; "pb-2" ]
                style [ style.color Colour.header; style.backgroundColor Colour.background ]
            ]

            h1 [
                text "Store Policies"

                classes [ "text-5xl"; "mt-12" ]

                style [ style.fontFamily "the-seasons"; style.color Colour.header ]
            ]

            Accordion [
                custom (
                    "onSelectionChange",
                    fun (keys: string list) ->
                        printfn "%A" keys
                        setSelected (Seq.toList keys |> List.tryExactlyOne)
                )

                classes [ "mt-8" ]

                children [
                    accordionItem "Shipping" ".0" [
                        ul [
                            classes [ "list-disc"; "list-outside"; "font-light"; "ml-4" ]
                            children [
                                li [
                                    classes [ "text-sm" ]
                                    children [
                                        Html.span [
                                            classes [ "text-base" ]
                                            children [ Html.span "Shipping is "; b "free across the US and Canada." ]
                                        ]
                                    ]
                                ]
                                li [
                                    classes [ "text-sm" ]
                                    children [
                                        Html.span [
                                            classes [ "text-base" ]
                                            children [
                                                Html.span
                                                    "We pack & ship your orders as soon as possible. Delivery typically takes "
                                                b "7-10 business days"
                                                Html.span
                                                    " from the time you receive your tracking number, though it may vary during holidays (we blame the reindeer traffic)."
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]

                    accordionItem "Returns & Exchanges" ".1" [
                        p "Once you receive your jewelry, take all the time you need to fall in love:"
                        br []
                        p [
                            Html.span "ZORI offers "
                            b "180 days"
                            Html.span " to exchange or "
                            b "90 days"
                            Html.span " to return your product for "
                            b "a full refund"
                            Html.span " for all orders throughout the US and Canada."
                        ]
                        br []
                        p "A few simple rules to keep the process smooth:"
                        br []
                        ul [
                            classes [ "list-decimal"; "list-outside"; "font-light"; "ml-4" ]
                            children [
                                li "For hygene reasons, earrings must be unworn when shipped back to us."
                                li "Items must be in their original condition, without wear or damage."
                                li
                                    "Items should be shipped back in their original packaging, including the Certificate of Authenticity, pouch, and jewelry box."
                            ]
                        ]
                        br []
                        p
                            "Once your shipment reaches us, it will go through a quality inspection. If an item doesn't meet the above guidelines, we won't be able to return or exchange it and will dispose of it responsibly for health & safety reasons."
                    ]

                    accordionItem "How to Return or Exchange" ".2" [
                        ul [
                            classes [ "list-decimal"; "list-outside"; "font-light"; "ml-4" ]
                            children [
                                li [
                                    Html.span "Use the "
                                    b "return label"
                                    Html.span " included in the package to send your item back to us."
                                ]
                                li [
                                    Html.span "Once we receive your package, we'll process it "
                                    b "as soon as possible."
                                ]
                                li [
                                    Html.span "Refunds will typically appear in your account within "
                                    b "5-10 business days."
                                ]
                            ]
                        ]
                    ]
                ]
            ]

            Divider []

            p [
                classes [ "text-center"; "mt-8"; "font-normal" ]
                style [ style.color Colour.paragraph ]

                children [
                    Html.text "Need assistance? Drop us a message at"
                    br []
                    a [
                        text "support@zorijewelry.com"
                        href "mailto:support@zorijewelry.com?subject=ZORI Policy Question&body=Hello,"
                        className "font-normal"
                    ]
                ]
            ]

            Button [
                custom ("radius", "full")
                style [
                    style.backgroundColor Colour.button.background
                    style.color Colour.button.foreground
                ]
                classes [ "mt-4"; "font-medium" ]
                text "Contact Support"
                custom (
                    "onPress",
                    fun _ ->
                        window.location.assign "mailto:support@zorijewelry.com?subject=ZORI Policy Question&body=Hello,"
                )
            ]

            div [ className "flex-grow" ]

            Footer()
        ]
    ]
