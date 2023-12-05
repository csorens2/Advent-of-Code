module Tests

open System
open Xunit
open Day5

[<Fact>]
let ``Part1 Example Test`` () = 
    let input = ParseInput("ExamplePart1.txt")
    Assert.Equal(35L, Part1 input)

[<Fact>]
let ``Part1 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(227653707L, Part1 input)

[<Fact>]
let ``Part2 Example Test`` () = 
    let input = ParseInput("ExamplePart2.txt")
    //Assert.Equal(, Part2 input)
    ()

[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    //Assert.Equal(, Part2 input)
    ()