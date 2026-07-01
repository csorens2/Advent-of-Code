module Tests

open Xunit
open Day11

[<Fact>]
let ``Part1 Example`` () = 
    let input = ParseInput("Example1.txt")
    Assert.Equal(5L, Part1 input)

[<Fact>]
let ``Part1 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(523L, Part1 input)

[<Fact>]
let ``Part2 Example`` () = 
    let input = ParseInput("Example2.txt")
    Assert.Equal(2L, Part2 input)

[<Fact>]
let ``Part2 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(517315308154944L, Part2 input)