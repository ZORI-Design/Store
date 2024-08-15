module Store.Shared.DomainModel

open System
open System.Text.RegularExpressions

type Marker = interface end

[<Measure>]
type mg // Milligram



type Mass = uint<mg>

type Quantity = | Quantity of int

let quantity q =
    if q > 0 && q < 1000 then Some <| Quantity q else None

type PhoneNumber = { DialingCode: string; Number: string }

type EmailAddress = | EmailAddress of string

let emailAddress (ea: string) =
    if
        ea.Length > 3
        && ea.Contains('@')
        && ea.LastIndexOf('.') > ea.LastIndexOf('@')
        && ea.LastIndexOf('.') < ea.Length - 2
    then
        Some <| EmailAddress ea
    else
        None

type AdPlatform =
    | Alphabet
    | Meta

type CampaignType =
    | OnlineAd of AdPlatform
    | OnlinePost
    | Mail
    | InPerson

type Market =
    | USA
    | Canada

type Campaign = {
    Name: string
    Type: CampaignType
    Start: DateOnly
    End: DateOnly
}

type Impression = { Region: Market option; Time: DateTimeOffset; Campaign: Campaign }

type EmailInteractionData = {
    Subject: string
    Body: string
    Sent: DateTimeOffset
    Sender: EmailAddress
    Recipients: EmailAddress list
}

type CartModificationAction =
    | AddedToCart
    | RemovedFromCart

type Material =
    | Gold14k
    | WhiteGold14k

type MoneyAmount =
    | USD of decimal
    | CAD of decimal
    | RMB of decimal

type ProductCategory =
    | Earring
    | Ring
    | Necklace

type ProductCollection = { Name: string; Description: string }

type Product = {
    Name: string
    Description: string
    Mass: Mass
    Plating: Material
    Price: MoneyAmount
    ProductionCost: MoneyAmount
    Thumbnail: Uri
    Assets: Uri list
    Category: ProductCategory
    Collection: ProductCollection
}

type CartInfo = { Items: (Product * Quantity) list; LastUpdated: DateTimeOffset }

type Cart =
    | EmptyCart
    | ActiveCart of CartInfo
    | AbandonedCart of CartInfo

type BrowserFormFactor =
    | MobileBrowser
    | DesktopBrowser

type BrowserData = {
    Browser: string
    FormFactor: BrowserFormFactor
    ScreenRes: uint * uint
    Languages: string list
}

type PageLoadData = { PageUrl: Uri; Browser: BrowserData }

type PositionData = { X: uint; Y: uint }

type WebsiteInteraction =
    | MousePosition of PositionData * TimeSpan
    | ScrollPosition of PositionData * TimeSpan
    | MouseClick of PositionData

type InteractionType =
    | EmailInteraction of EmailInteractionData
    | CartModification of CartModificationAction
    | PurchaseInitiated of CartInfo
    | PageLoad of PageLoadData
    | WebsiteInteraction of WebsiteInteraction

type Interaction =
    | ClientInteraction of InteractionType * DateTimeOffset
    | CompanyInteraction of InteractionType * DateTimeOffset

type Lead = { Impression: Impression; Interactions: Interaction list; Cart: Cart }

type CustomerName = { FirstName: string; LastName: string }

type StateOrProvince = | StateOrProvince of string

let usState (s: string) =
    if s.Length = 2 && Seq.forall Char.IsUpper s then
        Some <| StateOrProvince s
    else
        None

let caProvince = usState

type ZipCode = | ZipCode of string

let zipCode (z: string) =
    if z.Length = 5 && Seq.forall Char.IsNumber z then
        Some <| ZipCode z
    else
        None

type Postcode = | Postcode of string

let postcode (p: string) =
    let p = Regex.Replace(p, @"\s+", "").ToUpperInvariant()

    if
        p.Length = 6 && List.forall Char.IsLetter [ p[0]; p[2]; p[4] ] && List.forall Char.IsDigit [ p[1]; p[3]; p[5] ]
    then
        Some <| Postcode p
    else
        None

type UsStreetAddressData = {
    StreetAddress: string
    Unit: string option
    City: string
    State: StateOrProvince
    ZipCode: ZipCode
}

type CanadaStreetAddressData = {
    StreetAddress: string
    Unit: string option
    City: string
    Province: StateOrProvince
    Postcode: Postcode
}

type StreetAddress =
    | UsStreetAddress of UsStreetAddressData
    | CanadaStreetAddress of CanadaStreetAddressData

type OrderData = { Items: (Product * Quantity) list; OrderNumber: uint; TrackingNumber: string option }

type OrderState =
    | PendingOrder
    | PlacedOrder
    | CancelledOrder
    | FulfilledOrder
    | ReturnedOrder

type Order = (OrderState * DateTimeOffset) list * OrderData

/// Change from Pending/Placed to Cancelled.
type CancelOrder = Order -> Order

/// Change from Fulfilled to Returned.
type ReturnOrder = Order -> Order

type Customer = {
    Name: CustomerName
    Address: StreetAddress
    Email: EmailAddress
    Phone: PhoneNumber option
    OrderHistory: Order list
}

type User =
    | Lead of Lead
    | Customer of Customer

type Transaction = { Processed: DateTimeOffset; Amount: MoneyAmount; Order: Order }

type PaymentPlan = {
    Customer: Customer
    Order: Order
    Balance: MoneyAmount
    RemainingInstallments: uint
    TotalInstallments: uint
    Transactions: Transaction list
}

type Catalogue = Product list

type Stock = (Product * Quantity) list
