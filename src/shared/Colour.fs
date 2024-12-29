module Store.Shared.Colour

let primitive = {|
    Grey = {|
        ``100`` = "#FFFFFF"
        ``200`` = "#F7F6F5"
        ``300`` = "#DCDFE5"
        ``400`` = "#848992"
        ``500`` = "#525966"
        ``600`` = "#212121"
    |}
|}

let background = primitive.Grey.``200``

let dropdown = primitive.Grey.``400``

let paragraph = primitive.Grey.``500``

let header = primitive.Grey.``600``

let icon = primitive.Grey.``600``

let button = {|
    background = primitive.Grey.``600``
    foreground = primitive.Grey.``100``
|}