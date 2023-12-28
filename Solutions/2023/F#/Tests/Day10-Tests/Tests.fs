module Tests

open Xunit
open Day10

[<Fact>]
let ``Part1-1 Example Test`` () = 
    let input = ParseInput("ExamplePart1-1.txt")
    Assert.Equal(4, Part1 input)

[<Fact>]
let ``Part1-2 Example Test`` () = 
    let input = ParseInput("ExamplePart1-2.txt")
    Assert.Equal(4, Part1 input)

[<Fact>]
let ``Part1-3 Example Test`` () = 
    let input = ParseInput("ExamplePart1-3.txt")
    Assert.Equal(8, Part1 input)

[<Fact>]
let ``Part1-4 Example Test`` () = 
    let input = ParseInput("ExamplePart1-4.txt")
    Assert.Equal(8, Part1 input)

[<Fact>]
let ``Part1 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(6890, Part1 input)

[<Fact>]
let ``Part2-1 Example Test`` () = 
    let input = ParseInput("ExamplePart2-1.txt")
    Assert.Equal(4, Part2 input)

[<Fact>]
let ``Part2-2 Example Test`` () = 
    let input = ParseInput("ExamplePart2-2.txt")
    Assert.Equal(8, Part2 input)

[<Fact>]
let ``Part2-3 Example Test`` () = 
    let input = ParseInput("ExamplePart2-3.txt")
    Assert.Equal(10, Part2 input)

(*
[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    //Assert.Equal(, Part2 input)
    Assert.Fail("Not implemented")
*)