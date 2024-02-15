module Tests

open Xunit
open Day12

[<Fact>]
let ``Part1 Example Test`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(21UL, Part1 input)

[<Fact>]
let ``Part1 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(6981UL, Part1 input)

[<Fact>]
let ``Part2 Example Test`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(525152UL, Part2 input)

[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(4546215031609UL, Part2 input)
