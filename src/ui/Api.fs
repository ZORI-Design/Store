module Store.UI.Api

open Store.Shared.DomainModel
open Thoth.Json
open Fetch

let getCatalogue (setCatalog: Catalogue -> unit) : unit =
    fetch "https://api.zorijewelry.com/stock" []
    |> Promise.bind _.json()
    |> Promise.map (fun i -> i :?> Catalogue)
    |> Promise.iter setCatalog

let logLoad (url: string, browser: BrowserData) =
    let interaction = ClientInteraction (
        PageLoad {
            PageUrl = url
            Browser = browser
        },
        System.DateTimeOffset.UtcNow
    )

    fetch "https://api.zorijewelry.com/interaction" [
        Method HttpMethod.POST
        requestHeaders [ ContentType "application/json" ]
        Body <| unbox (Encode.Auto.toString interaction)
    ]