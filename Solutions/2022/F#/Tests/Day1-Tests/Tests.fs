module Tests

open Xunit
open Day1

[<Fact>]
let ``Part1 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(24000, Part1 input)

[<Fact>]
let ``Part1 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(67633, Part1 input)

[<Fact>]
let ``Part2 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(45000, Part2 input)

[<Fact>]
let ``Part2 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(199628, Part2 input)
