module Day6

open System
open System.IO
open System.Text.RegularExpressions

type Race = {
    Time: int64
    RecordDistance: int64
}

let ParseInput filepath = 
    let lines = 
        File.ReadLines(filepath)
        |> Seq.toList

    let numRegex = Regex(@"(\d+)")
    let times = 
        numRegex.Matches (List.item 0 lines)
        |> Seq.map (fun numMatch -> numMatch.Groups.[1].Value)
    let distances = 
        numRegex.Matches (List.item 1 lines)
        |> Seq.map (fun distanceMatch -> distanceMatch.Groups.[1].Value)

    distances
    |> Seq.zip times
    |> Seq.toList

/// By the nature of how performance scales, the highest performance hold-time will always be half of the total time. 
/// So we can find out how many variants beat the record by: 
///
/// 1. Binary searching for the minimum hold-time necessary between 0 and the midpoint to beat the record. 
/// 
/// 2. Get the difference between this minimum and the midpoint, and multiply it by two to handle both sides.
let GetRecordBreakingHoldVariations (race: Race) = 
    let rec findMinimumHoldTime leftBound rightBound : int64 option = 
        let midPoint = (leftBound + rightBound) / 2L
        let midPointDistance = ((race.Time - midPoint) * midPoint)
        let midPointBeatsRecord = race.RecordDistance < midPointDistance
        match leftBound = rightBound with 
        | true -> 
            // Need to account for no record-breaking variants existing
            match midPointBeatsRecord with 
            | true -> Some midPoint
            | false -> None
        | false ->
            match midPointBeatsRecord with 
            | true -> findMinimumHoldTime leftBound midPoint
            | false -> findMinimumHoldTime (midPoint + 1L) rightBound

    let peakHoldTime = race.Time / 2L
    match findMinimumHoldTime 0L peakHoldTime with 
    | None -> 0L
    | Some minHoldTime -> 
        let nonPeakVariants = (peakHoldTime - minHoldTime) * 2L   
        match race.Time % 2L = 0L with 
        | true -> nonPeakVariants + 1L
        | false -> nonPeakVariants + 2L

let Part1 input =     
    input
    |> List.map (fun (time, distance) -> {Race.Time = int64 time; RecordDistance = int64 distance})
    |> List.map (fun race -> GetRecordBreakingHoldVariations race)
    |> List.fold (fun acc winningMethods -> acc * winningMethods) 1L
    
let Part2 (input: (string*string) list) = 
    let (raceTimeString, raceDistanceString) =
        input
        |> List.fold (fun (timeAcc, distanceAcc) (nextTime, nextDistance) -> (timeAcc + nextTime, distanceAcc + nextDistance)) (String.Empty, String.Empty)

    GetRecordBreakingHoldVariations {Race.Time = int64 raceTimeString; RecordDistance = int64 raceDistanceString} 

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 6209190
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 28545089
    0