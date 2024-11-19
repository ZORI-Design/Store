module Store.UI.Api

open Store.Shared.DomainModel
open Thoth.Json
open Fetch

let getCatalogue (callback: Catalogue -> unit) : unit =
    fetch "https://api.zorijewelry.com/stock/catalogue" []
    |> Promise.bind _.text()
    |> Promise.map (fun s -> Decode.Auto.fromString<Catalogue>(s, extra = (Extra.empty |> Extra.withDecimal)))
    |> Promise.iter (Result.map callback >> printfn "%A")

let logLoad (url: string, browser: BrowserData) =
    let interaction =
        ClientInteraction(PageLoad { PageUrl = url; Browser = browser }, System.DateTimeOffset.UtcNow)

    fetch "https://api.zorijewelry.com/interaction" [
        Method HttpMethod.POST
        requestHeaders [ ContentType "application/json" ]
        Body <| unbox (Encode.Auto.toString interaction)
    ]
