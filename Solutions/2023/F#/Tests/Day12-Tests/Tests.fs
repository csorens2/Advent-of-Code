module Tests

open Xunit
open Day12

[<Fact>]
let ``Part1 Example Test`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(21, Part1 input)

[<Fact>]
let ``Part1 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(6981, Part1 input)

[<Fact>]
let ``Part2 Example Test`` () = 
    let input = ParseInput("Example.txt")
    //Assert.Equal(, Part2 input)
    Assert.Fail("Not implemented")

[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    //Assert.Equal(, Part2 input)
    Assert.Fail("Not implemented")