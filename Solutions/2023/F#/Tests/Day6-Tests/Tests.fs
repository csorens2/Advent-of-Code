module Tests

open Xunit
open Day6

[<Fact>]
let ``GetRecordBreakingHoldVariations returns 0 when no record-breaking variants exist`` () = 
    let testRace = {Race.Time = 10L; RecordDistance = System.Int64.MaxValue}
    Assert.Equal(0L, GetRecordBreakingHoldVariations testRace)

[<Fact>]
let ``GetRecordBreakingHoldVariations returns correct record-breaking variants for even race times`` () = 
    let testRace = {Race.Time = 10L; RecordDistance = 22L}
    Assert.Equal(3L, GetRecordBreakingHoldVariations testRace)

[<Fact>]
let ``GetRecordBreakingHoldVariations returns correct record-breaking variants for odd race times`` () = 
    let testRace = {Race.Time = 11L; RecordDistance = 25L}
    Assert.Equal(4L, GetRecordBreakingHoldVariations testRace)

[<Fact>]
let ``Part1 Example Test`` () = 
    let input = ParseInput("ExamplePart1.txt")
    Assert.Equal(288L, Part1 input)

[<Fact>]
let ``Part1 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(6209190L, Part1 input)

[<Fact>]
let ``Part2 Example Test`` () = 
    let input = ParseInput("ExamplePart2.txt")
    Assert.Equal(71503L, Part2 input)

[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(28545089L, Part2 input)