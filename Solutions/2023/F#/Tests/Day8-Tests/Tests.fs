module Tests

open Xunit
open Day8

[<Fact>]
let ``Part1-1 Example Test`` () = 
    let input = ParseInput("ExamplePart1-1.txt")
    Assert.Equal(2, Part1 input)

[<Fact>]
let ``Part1-2 Example Test`` () = 
    let input = ParseInput("ExamplePart1-2.txt")
    Assert.Equal(6, Part1 input)

[<Fact>]
let ``Part1 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(11911, Part1 input)

[<Fact>]
let ``Part2 Example Test`` () = 
    let input = ParseInput("ExamplePart2.txt")
    Assert.Equal(6, Part2 input)

[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    //Assert.Equal(, Part2 input)
    Assert.Fail ("Not implemented")