module Store.Test.Meta

open Xunit

type FunctionComponent = unit -> Fable.React.ReactElement

[<Fact>]
let ``Ensure Test Runner is Functioning`` () =
    Assert.True(true)
    Assert.False(false)
    Assert.Equal(true, true)
    Assert.NotEqual(true, false)
    Assert.Equal({| a = true; b = false |}, {| a = true; b = false |})
    Assert.NotSame({| a = true; b = false |}, {| a = true; b = false |})

[<Fact>]
let ``Ensure Test Runner can Access the Store Project`` () =
    Assert.IsAssignableFrom<FunctionComponent>(Store.UI.Root.Root)
