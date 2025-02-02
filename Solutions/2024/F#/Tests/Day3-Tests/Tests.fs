module Tests

open Xunit
open Day3

[<Fact>]
let ``Part1 Example`` () = 
    let input = ParseInput("Example1.txt")
    Assert.Equal(161, Part1 input)

[<Fact>]
let ``Part1 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(184122457, Part1 input)

[<Fact>]
let ``Part2 Example`` () = 
    let input = ParseInput("Example2.txt")
    Assert.Equal(48, Part2 input)

[<Fact>]
let ``Part2 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(107862689, Part2 input)
