module Tests

open Xunit
open Day11

[<Fact>]
let ``Part1 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(55312L, Part1 input)

[<Fact>]
let ``Part1 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(186175L, Part1 input)

[<Fact>]
let ``Part2 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(220566831337810L, Part2 input)
