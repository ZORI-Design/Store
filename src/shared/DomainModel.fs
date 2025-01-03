﻿module Store.Shared.DomainModel

open System
open System.Text.RegularExpressions

type Marker = interface end

[<Measure>]
type mg // Milligram



type Mass = uint<mg>

type Quantity =
    | Quantity of int
    | OutOfStock

let quantity =
    function
    | 0 -> Some OutOfStock
    | q when q > 0 && q < 1000 -> Some(Quantity q)
    | _ -> None

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

type Url = string
type CorrelationId = string

let correlationId (num: uint64) = num.ToString("X32")

type DateOnly = | DateOnly of year: int * month: int * day: int

let dateOnly (year: int, month: int, day: int) : DateOnly option =
    let maxDay =
        match month with
        | 1
        | 3
        | 5
        | 7
        | 8
        | 10
        | 12 -> 31
        | 2 when (year % 4 = 0 && year % 100 <> 0) || year % 400 = 0 -> 29
        | 2 -> 28
        | 4
        | 6
        | 9
        | 11 -> 30
        | _ -> 0

    if year > 0 && year < 10000 && month > 0 && month <= 12 && day > 0 && day <= maxDay then
        DateOnly(year, month, day) |> Some
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
    Thumbnail: Url
    Assets: Url list
    Category: ProductCategory
    Collection: ProductCollection
}

type CartInfo = { Items: (Product * Quantity) list; LastUpdated: DateTimeOffset }

type Cart =
    | EmptyCart
    | ActiveCart of CartInfo
    | AbandonedCart of CartInfo

type Lead = { Impression: Impression; CorrelationId: CorrelationId; Cart: Cart }

type BrowserFormFactor =
    | MobileBrowser
    | DesktopBrowser

type BrowserData = {
    CorrelationId: CorrelationId
    Browser: string
    FormFactor: BrowserFormFactor
    ScreenRes: uint * uint
    Languages: string list
}

type PageLoadData = { PageUrl: Url; Browser: BrowserData }

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
    CorrelationId: CorrelationId
}

type User =
    | Lead of Lead
    | Customer of Customer

type Transaction = { Processed: DateTimeOffset; Amount: MoneyAmount }

type PaymentPlan = {
    Customer: Customer
    Order: Order
    Balance: MoneyAmount
    RemainingInstallments: uint
    TotalInstallments: uint
    Transactions: Transaction list
}

type PaymentUpdate =
    | TransactionMade of Transaction
    | StatusUpdated of OrderState
    | TrackingNumberCreated of string
    | CustomerDataUpdated of Customer

type Catalogue = Product seq

type Stock = (Product * Quantity) seq
