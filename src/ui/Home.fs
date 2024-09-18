module Store.UI.Home

open Feliz
open Fable.Core.JsInterop
open Feliz.Router

open type Feliz.Html
open type Feliz.prop
open type NextUI

[<ReactComponent>]
let Home () =
    div [
        children [
            video [
                style [
                    style.position.fixedRelativeToWindow
                    length.perc 100 |> style.minHeight
                    length.perc 100 |> style.minWidth
                ]
                playsInline true
                autoPlay true
                muted true
                loop true
                id "hero-video"
                children [
                    source [ src "./assets/hero-animation.webm"; type' "video/webm" ]
                    Html.text "Your browser does not support the video tag."
                ]
            ]

            div [
                style [
                    style.position.absolute
                    length.perc 100 |> style.minHeight
                    length.perc 100 |> style.minWidth
                    style.backgroundColor "rgba(25, 35, 75, 0.25)"
                ]
                children [
                    div [
                        style [
                            length.vh 39 |> style.marginTop
                            style.textAlign.center
                            style.fontFamily "the-seasons"
                            length.vw 4.2 |> style.fontSize
                            style.color "white"
                        ]
                        children [ Html.text "Just A Bit Different." ]
                    ]

                    div [
                        style [
                            length.vh 1.8 |> style.marginTop
                            style.textAlign.center
                            style.fontFamily "Figtree"
                            style.color "black"
                        ]
                        children [
                            Button [
                                style [ style.backgroundColor "white"; style.fontWeight 550 ]
                                custom ("size", "lg")
                                custom ("radius", "full")
                                text "Shop Now"
                            ]
                        ]
                    ]

                    div [
                        className [ "sm:flex"; "gap-2" ]
                        prop.onClick (fun _ -> Router.navigatePath "/about")
                        style [
                            length.vh 3 |> style.marginTop
                            style.fontFamily "Figtree"
                            style.fontWeight 400
                            // The following insanity is because Safari:
                            style.display.flex
                            style.custom ("-webkit-justify-content", "center")
                            style.justifyContent.center
                            style.custom ("-webkit-align-items", "center")
                            style.alignItems.center
                        ]
                        children [
                            Link [
                                className [ "text-small" ]
                                text "Hear Our Story"
                                prop.onClick (fun _ -> Router.navigatePath "/about")
                                style [ style.cursor "pointer"; style.color "white" ]
                            ]
                            img [
                                prop.src <| import "default" "./assets/right-arrow.svg"
                                prop.style [
                                    length.perc 0.1 |> style.marginTop
                                    length.px 10 |> style.height
                                    style.cursor "pointer"
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]
