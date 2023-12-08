module Tests

open Day5
open Xunit
open System

[<Fact>]
let ``RangesOverlap Returns True for Overlapping Ranges`` () = 
    let baseStart = 0L
    let baseRange = 10L
    let rangeBase = (baseStart, baseRange)

    let shift = baseRange / 2L
    let testRangeLeft = (baseStart - shift, baseRange)
    let testRangeRight = (baseStart + shift, baseRange)
    let testRangeTotalCover = (baseStart - shift, baseRange + shift)

    Assert.True(RangesOverlap rangeBase testRangeLeft)
    Assert.True(RangesOverlap rangeBase testRangeRight)
    Assert.True(RangesOverlap rangeBase testRangeTotalCover)

[<Fact>]
let ``RangesOverlp Returns False for NonOverlapping Ranges`` () = 
    let baseStart = 0L
    let baseRange = 10L
    let rangeBase = (baseStart, baseRange)

    let testRangeLeft = (baseStart - baseRange, baseRange)
    let testRangeRight = (baseStart + baseRange, baseRange)

    Assert.False(RangesOverlap rangeBase testRangeLeft)
    Assert.False(RangesOverlap rangeBase testRangeRight)

[<Fact>]
let ``MapSeedRange Properly Maps Seeds`` () =
    let baseStart = 0L
    let baseRange = 10L
    let baseSeed = {SeedRange.Start = baseStart; Range = baseRange}

    let shift = baseRange / 2L
    let testDestination = 20L

    // Mapped left, unmapped right
    let testLeft = 
        (
            [{MapEntry.SourceStart = baseStart - shift; Range = baseRange; DestinationStart = testDestination}],
            [
                {SeedRange.Start = testDestination + shift; Range = baseRange - shift}
                {SeedRange.Start = baseStart + shift; Range = baseRange - shift}
            ]
        )
    // Unmapped left, mapped right
    let testRight = 
        (
            [{MapEntry.SourceStart = baseStart + shift; Range = baseRange; DestinationStart = testDestination}],
            [
                {SeedRange.Start = baseStart; Range = baseRange - shift}
                {SeedRange.Start = testDestination; Range = baseRange - shift}
            ]
        )

    // Unmapped left, mapped center, unmapped right
    // Also tests "chaining"
    let testCenter = 
        (
            let leftRightRange = baseRange / 3L
            let middleRange = baseRange - (leftRightRange * 2L)
            [{MapEntry.SourceStart = baseStart + leftRightRange; Range = middleRange; DestinationStart = testDestination}],
            [
                {SeedRange.Start = baseStart; Range = leftRightRange}
                {SeedRange.Start = testDestination; Range = middleRange}
                {SeedRange.Start = baseStart + leftRightRange + middleRange; Range = leftRightRange}
            ]
        )

    // Mapped whole
    let testWhole = 
        (
            [{MapEntry.SourceStart = baseStart - shift; Range = baseRange * 2L; DestinationStart = testDestination}],
            [
                {SeedRange.Start = testDestination + shift; Range = baseRange}
            ]
        )

    // Mapped nothing
    let testNothing = 
        (
            [{MapEntry.SourceStart = System.Int64.MinValue; Range = 1L; DestinationStart = 20L}],
            [
                baseSeed
            ]
        )

    let testCases = [
        (testLeft, nameof testLeft)
        (testRight, nameof testRight)
        (testCenter, nameof testCenter)
        (testWhole, nameof testWhole)
        (testNothing, nameof testNothing)
    ]

    for ((testMapEntries, testSeeds), subTestName) in testCases do
        let testResult = MapSeedRange testMapEntries baseSeed 
        for expectedSeed in testSeeds do 
            Assert.True(List.contains expectedSeed testResult, $"Failed test {subTestName}")

[<Fact>]
let ``Part1 Example Test`` () = 
    let input = ParseInput("ExamplePart1.txt")
    Assert.Equal(35L, Part1 input)

[<Fact>]
let ``Part1 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(227653707L, Part1 input)

[<Fact>]
let ``Part2 Example Test`` () = 
    let input = ParseInput("ExamplePart2.txt")
    Assert.Equal(46L, Part2 input)

[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(78775051L, Part2 input)