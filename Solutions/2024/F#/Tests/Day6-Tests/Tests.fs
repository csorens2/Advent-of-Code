module Tests

open Xunit
open Day6

[<Fact>]
let ``Part1 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(41, Part1 input)

[<Fact>]
let ``Part1 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(4696, Part1 input)

[<Fact>]
let ``Part2 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(6, Part2 input)

[<Fact>]
let ``Part2 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(1443, Part2 input)
