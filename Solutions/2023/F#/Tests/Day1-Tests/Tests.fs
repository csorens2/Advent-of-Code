module Tests

open Xunit
open Day1

[<Fact>]
let ``Part1 SubTest`` () = 
    let input = ParseInput("InputPart1.txt")
    Assert.Equal(142, (Part1 input))

[<Fact>]
let ``Part2 SubTest`` () = 
    let input = ParseInput("InputPart2.txt")
    Assert.Equal(281, (Part2 input))