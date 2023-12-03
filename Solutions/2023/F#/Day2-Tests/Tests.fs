module Tests

open Xunit
open Day2

[<Fact>]
let ``Part1 SubTest`` () = 
    let input = ParseInput("InputPart1.txt")
    Assert.Equal(8, Part1 input)

[<Fact>]
let ``Part2 SubTest`` () = 
    let input = ParseInput("InputPart2.txt")
    Assert.Equal(2286, Part2 input)