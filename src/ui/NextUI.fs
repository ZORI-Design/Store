namespace Store.UI

open Feliz
open Fable.Core.JsInterop
open Fable.React

type NextUI =
    [<ReactComponent(import = "NextUIProvider", from = "@nextui-org/react")>]
    static member NextUIProvider
        (properties: {| navigate: string -> unit; children: #seq<ReactElement> |})
        : ReactElement =
        React.imported ()

    static member Component (name: string) (xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import name "@nextui-org/react", createObj !!xs)

    // FOLLOW THIS PATTERN AS YOU NEED ADDITIONAL ELEMENTS!
    // Just change the function name from "Button" to whatever you need in both places.
    static member inline Button: IReactProperty list -> ReactElement = NextUI.Component "Button"
