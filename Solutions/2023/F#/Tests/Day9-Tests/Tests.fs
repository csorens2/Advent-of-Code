module Tests

open Xunit
open Day9

[<Fact>]
let ``Part1 Example Test`` () = 
    let input = ParseInput("ExamplePart1.txt")
    Assert.Equal(114, Part1 input)

[<Fact>]
let ``Part1 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(1972648895, Part1 input)

[<Fact>]
let ``Part2 Example Test`` () = 
    let input = ParseInput("ExamplePart2.txt")
    Assert.Equal(2, Part2 input)

[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(919, Part2 input)