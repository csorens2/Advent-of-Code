module Tests

open Xunit
open Day1

[<Fact>]
let ``Part1 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(11, Part1 input)

[<Fact>]
let ``Part1 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(1970720, Part1 input)

[<Fact>]
let ``Part2 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(31, Part2 input)

[<Fact>]
let ``Part2 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(17191599, Part2 input)
