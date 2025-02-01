﻿module Tests

open Xunit
open Day4

[<Fact>]
let ``Part1 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(18, Part1 input)

[<Fact>]
let ``Part1 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(2633, Part1 input)

(*
[<Fact>]
let ``Part2 Example`` () = 
    let input = ParseInput("Example.txt")
    //Assert.Equal(, Part2 input)
    Assert.Fail("Not implemented")

[<Fact>]
let ``Part2 Input`` () = 
    let input = ParseInput("Input.txt")
    //Assert.Equal(, Part2 input)
    Assert.Fail("Not implemented")
*)