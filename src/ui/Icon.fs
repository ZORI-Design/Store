namespace Store.UI

open Feliz
open Store.Shared

type Icon =
    static member inline Instagram =
        Svg.svg [
            svg.width 16
            svg.height 16
            svg.viewBox (0, 0, 16, 16)
            svg.fill "none"

            svg.children [
                Svg.path [
                    svg.custom ("fill-rule", "evenodd")
                    svg.clipRule.evenodd
                    svg.fill Colour.icon
                    svg.d
                        "M0.533325 4.9375C0.533325 2.35787 2.62453 0.266663 5.20416 0.266663H11.3292C13.9088 0.266663 16 2.35787 16 4.9375V11.0625C16 13.6421 13.9088 15.7333 11.3292 15.7333H5.20416C2.62453 15.7333 0.533325 13.6421 0.533325 11.0625V4.9375ZM5.20416 1.73333C3.43455 1.73333 1.99999 3.16788 1.99999 4.9375V11.0625C1.99999 12.8321 3.43455 14.2667 5.20416 14.2667H11.3292C13.0988 14.2667 14.5333 12.8321 14.5333 11.0625V4.9375C14.5333 3.16788 13.0988 1.73333 11.3292 1.73333H5.20416Z"
                ]
                Svg.path [
                    svg.custom ("fill-rule", "evenodd")
                    svg.clipRule.evenodd
                    svg.fill Colour.icon
                    svg.d
                        "M8.26666 5.62222C6.95345 5.62222 5.88888 6.68678 5.88888 7.99999C5.88888 9.3132 6.95345 10.3778 8.26666 10.3778C9.57987 10.3778 10.6444 9.3132 10.6444 7.99999C10.6444 6.68678 9.57987 5.62222 8.26666 5.62222ZM4.42221 7.99999C4.42221 5.87677 6.14343 4.15555 8.26666 4.15555C10.3899 4.15555 12.1111 5.87677 12.1111 7.99999C12.1111 10.1232 10.3899 11.8444 8.26666 11.8444C6.14343 11.8444 4.42221 10.1232 4.42221 7.99999Z"
                ]
                Svg.path [
                    svg.fill Colour.icon
                    svg.d
                        "M13.1555 4C13.1555 4.49092 12.7576 4.88889 12.2667 4.88889C11.7757 4.88889 11.3778 4.49092 11.3778 4C11.3778 3.50908 11.7757 3.11111 12.2667 3.11111C12.7576 3.11111 13.1555 3.50908 13.1555 4Z"
                ]
                Svg.path [
                    svg.custom ("fill-rule", "evenodd")
                    svg.clipRule.evenodd
                    svg.fill Colour.icon
                    svg.d
                        "M12.2667 4.88889C12.7576 4.88889 13.1555 4.49092 13.1555 4C13.1555 3.50908 12.7576 3.11111 12.2667 3.11111C11.7757 3.11111 11.3778 3.50908 11.3778 4C11.3778 4.49092 11.7757 4.88889 12.2667 4.88889Z"
                ]
            ]
        ]

//<svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
