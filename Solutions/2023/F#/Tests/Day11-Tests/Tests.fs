module Tests

open Xunit
open Day11

[<Fact>]
let ``Part1 Example Test`` () = 
    let input = ParseInput("ExamplePart1.txt")
    Assert.Equal(374UL, Part1 input)

[<Fact>]
let ``Part1 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(9565386UL, Part1 input)

[<Fact>]
let ``Part2-1 Example Test`` () = 
    let input = ParseInput("ExamplePart2.txt")
    Assert.Equal(1030UL, UniverseDistancesSum input 10)

[<Fact>]
let ``Part2-2 Example Test`` () = 
    let input = ParseInput("ExamplePart2.txt")
    Assert.Equal(8410UL, UniverseDistancesSum input 100)

[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(857986849428UL, Part2 input)