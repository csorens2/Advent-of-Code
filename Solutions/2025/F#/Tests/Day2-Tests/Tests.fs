module Tests

open Xunit
open Day2

[<Fact>]
let ``Part1 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(1227775554L, Part1 input)

[<Fact>]
let ``Part1 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(32976912643L, Part1 input)

[<Fact>]
let ``Part2 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(4174379265L, Part2 input)

[<Fact>]
let ``Part2 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(54446379122L, Part2 input)