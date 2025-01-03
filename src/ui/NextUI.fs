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

    static member inline color(colorName: string) = prop.custom ("color", colorName)
    static member inline justify(position: string) = prop.custom ("justify", position)

    // FOLLOW THIS PATTERN AS YOU NEED ADDITIONAL ELEMENTS!
    // Just change the function name from "Button" to whatever you need in both places.
    static member inline Button(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Button" "@nextui-org/react", createObj !!xs)

    static member inline Navbar(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Navbar" "@nextui-org/react", createObj !!xs)

    static member inline Navbar(xs: ReactElement list) : ReactElement = NextUI.Navbar [ prop.children xs ]

    static member inline NavbarBrand(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarBrand" "@nextui-org/react", createObj !!xs)

    static member inline NavbarContent(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarContent" "@nextui-org/react", createObj !!xs)

    static member inline NavbarContent(xs: ReactElement list) : ReactElement =
        NextUI.NavbarContent [ prop.children xs ]

    static member inline NavbarItem(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarItem" "@nextui-org/react", createObj !!xs)

    static member inline NavbarItem(xs: ReactElement list) : ReactElement = NextUI.NavbarItem [ prop.children xs ]

    static member inline NavbarMenuToggle(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarMenuToggle" "@nextui-org/react", createObj !!xs)

    static member inline NavbarMenu(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarMenu" "@nextui-org/react", createObj !!xs)

    static member inline NavbarMenuItem(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarMenuItem" "@nextui-org/react", createObj !!xs)

    static member inline Link(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Link" "@nextui-org/react", createObj !!xs)

    static member inline Dropdown(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Dropdown" "@nextui-org/react", createObj !!xs)

    static member inline DropdownTrigger(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "DropdownTrigger" "@nextui-org/react", createObj !!xs)

    static member inline DropdownMenu(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "DropdownMenu" "@nextui-org/react", createObj !!xs)

    static member inline DropdownItem(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "DropdownItem" "@nextui-org/react", createObj !!xs)

    static member inline Image(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Image" "@nextui-org/react", createObj !!xs)

    static member inline Accordion(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Accordion" "@nextui-org/react", createObj !!xs)

    static member inline Accordion(xs: ReactElement list) : ReactElement = NextUI.Accordion [ prop.children xs ]

    static member inline AccordionItem(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "AccordionItem" "@nextui-org/react", createObj !!xs)

    static member inline Divider(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Divider" "@nextui-org/react", createObj !!xs)
