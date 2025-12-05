module Tests

open Xunit
open Day4

[<Fact>]
let ``Part1 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(13, Part1 input)

[<Fact>]
let ``Part1 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(1508, Part1 input)

[<Fact>]
let ``Part2 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(43, Part2 input)

[<Fact>]
let ``Part2 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(8538, Part2 input)