namespace Store.UI

open Feliz
open Fable.Core.JsInterop
open Fable.React

type HeroUI =
    [<ReactComponent(import = "HeroUIProvider", from = "@heroui/react")>]
    static member HeroUIProvider
        (properties: {| navigate: string -> unit; children: #seq<ReactElement> |})
        : ReactElement =
        React.imported ()

    static member inline color(colorName: string) = prop.custom ("color", colorName)
    static member inline justify(position: string) = prop.custom ("justify", position)

    // FOLLOW THIS PATTERN AS YOU NEED ADDITIONAL ELEMENTS!
    // Just change the function name from "Button" to whatever you need in both places.
    static member inline Button(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Button" "@heroui/react", createObj !!xs)

    static member inline Navbar(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Navbar" "@heroui/react", createObj !!xs)

    static member inline Navbar(xs: ReactElement list) : ReactElement = HeroUI.Navbar [ prop.children xs ]

    static member inline NavbarBrand(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarBrand" "@heroui/react", createObj !!xs)

    static member inline NavbarContent(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarContent" "@heroui/react", createObj !!xs)

    static member inline NavbarContent(xs: ReactElement list) : ReactElement =
        HeroUI.NavbarContent [ prop.children xs ]

    static member inline NavbarItem(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarItem" "@heroui/react", createObj !!xs)

    static member inline NavbarItem(xs: ReactElement list) : ReactElement = HeroUI.NavbarItem [ prop.children xs ]

    static member inline NavbarMenuToggle(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarMenuToggle" "@heroui/react", createObj !!xs)

    static member inline NavbarMenu(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarMenu" "@heroui/react", createObj !!xs)

    static member inline NavbarMenuItem(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "NavbarMenuItem" "@heroui/react", createObj !!xs)

    static member inline Link(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Link" "@heroui/react", createObj !!xs)

    static member inline Dropdown(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Dropdown" "@heroui/react", createObj !!xs)

    static member inline DropdownTrigger(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "DropdownTrigger" "@heroui/react", createObj !!xs)

    static member inline DropdownMenu(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "DropdownMenu" "@heroui/react", createObj !!xs)

    static member inline DropdownItem(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "DropdownItem" "@heroui/react", createObj !!xs)

    static member inline Image(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Image" "@heroui/react", createObj !!xs)

    static member inline Accordion(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Accordion" "@heroui/react", createObj !!xs)

    static member inline Accordion(xs: ReactElement list) : ReactElement = HeroUI.Accordion [ prop.children xs ]

    static member inline AccordionItem(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "AccordionItem" "@heroui/react", createObj !!xs)

    static member inline Divider(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Divider" "@heroui/react", createObj !!xs)

    static member inline Chip(xs: IReactProperty list) : ReactElement =
        Interop.reactApi.createElement (import "Chip" "@heroui/react", createObj !!xs)
