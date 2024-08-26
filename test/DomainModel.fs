module Store.Test.DomainModel

open Xunit
open Microsoft.FSharp.Reflection
open Store.Shared
open System.Reflection

let domainModel = typeof<DomainModel.Marker>.DeclaringType

[<Fact>]
let ``Domain Model is a Module`` () = FSharpType.IsModule domainModel

[<Fact>]
let ``Discriminated Unions Have Up To Five Cases`` () =
    domainModel.GetNestedTypes(BindingFlags.Public)
    |> Array.filter FSharpType.IsUnion
    |> Array.map FSharpType.GetUnionCases
    |> Array.iter (fun t -> Assert.InRange(t.Length, 1, 5))

[<Fact>]
let ``Records Have Up To Ten Members`` () =
    domainModel.GetNestedTypes(BindingFlags.Public)
    |> Array.filter FSharpType.IsRecord
    |> Array.map FSharpType.GetRecordFields
    |> Array.iter (fun t -> Assert.InRange(t.Length, 1, 10))
