module Tests

open Xunit
open Day13

[<Fact>]
let ``Part1 Example Test`` () = 
    let input = ParseInput("ExamplePart1.txt")
    Assert.Equal(405, Part1 input)

[<Fact>]
let ``Part1 Input Test`` () = 
    let input = ParseInput("Input.txt")
    //Assert.Equal(, Part1 input)
    Assert.Fail("Not implemented")

[<Fact>]
let ``Part2 Example Test`` () = 
    let input = ParseInput("ExamplePart2.txt")
    //Assert.Equal(, Part2 input)
    Assert.Fail("Not implemented")

[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    //Assert.Equal(, Part2 input)
    Assert.Fail("Not implemented")