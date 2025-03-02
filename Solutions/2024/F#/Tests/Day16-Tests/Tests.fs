module Tests

open Xunit
open Day16

[<Fact>]
let ``Part1 Example1`` () = 
    let input = ParseInput("Example1.txt")
    Assert.Equal(7036, Part1 input)

[<Fact>]
let ``Part1 Example2`` () = 
    let input = ParseInput("Example2.txt")
    Assert.Equal(11048, Part1 input)

[<Fact>]
let ``Part1 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(102460, Part1 input)

(*
[<Fact>]
let ``Part2 Example`` () = 
    let input = ParseInput("Example2.txt")
    //Assert.Equal(, Part2 input)
    Assert.Fail("Not implemented")

[<Fact>]
let ``Part2 Input`` () = 
    let input = ParseInput("Input.txt")
    //Assert.Equal(, Part2 input)
    Assert.Fail("Not implemented")
*)