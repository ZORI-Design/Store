module Store.UI.Api

open Store.Shared.DomainModel
open Thoth.Json
open Fetch

let getCatalogue (callback: Catalogue -> unit) : unit =
    fetch "https://api.zorijewelry.com/stock/catalogue" []
    |> Promise.bind _.text()
    |> Promise.map (fun i -> printfn "%s" i; Decode.Auto.fromString<Catalogue> i)
    |> Promise.iter (Result.map callback >> ignore)

let logLoad (url: string, browser: BrowserData) =
    let interaction =
        ClientInteraction(PageLoad { PageUrl = url; Browser = browser }, System.DateTimeOffset.UtcNow)

    fetch "https://api.zorijewelry.com/interaction" [
        Method HttpMethod.POST
        requestHeaders [ ContentType "application/json" ]
        Body <| unbox (Encode.Auto.toString interaction)
    ]
