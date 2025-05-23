﻿module Tests

open Xunit
open Day15

[<Fact>]
let ``Part1 Example`` () = 
    let input = ParseInput("Example.txt")
    Assert.Equal(1320, Part1 input)

[<Fact>]
let ``Part1 Input`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(509152, Part1 input)

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