module Day12

open System.IO
open System.Text.RegularExpressions

type Spring = 
    | Operational 
    | Damaged
    | Unknown

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map (fun line ->
        let springs = 
            Regex(@"([#.?]+)").Match(line).Value
            |> Seq.map(fun springChar ->
                match springChar with 
                | '.' -> Operational
                | '#' -> Damaged
                | '?' -> Unknown
                | _ -> failwith $"Failed parsing unknown char {springChar}")
            |> Seq.toList
        let conditionRecord = 
            Regex(@"(\d+)").Matches(line)
            |> Seq.map (fun recordMatch -> int recordMatch.Groups.[0].Value)
            |> Seq.toList
        (springs, conditionRecord))
    |> Seq.toList

let Part1 input = 
    let rec springVariants remainingSprings remainingGroups currentGroupLength = 
        match remainingSprings with 
        | [] ->
            match remainingGroups with 
            | [] -> 1
            | [lastRemainingGroup] when currentGroupLength = lastRemainingGroup -> 1
            | _ -> 0
        | nextSpring :: nextRemainingSprings ->
            let nextGroup = List.tryHead remainingGroups
            match nextSpring with 
            | Operational when currentGroupLength = 0 -> springVariants nextRemainingSprings remainingGroups currentGroupLength
            | Operational when Option.isSome nextGroup && currentGroupLength = nextGroup.Value -> springVariants nextRemainingSprings (List.tail remainingGroups) 0
            | Operational -> 0
            | Damaged when Option.isSome nextGroup && currentGroupLength < nextGroup.Value -> springVariants nextRemainingSprings remainingGroups (currentGroupLength + 1)
            | Damaged -> 0
            | Unknown -> 
                (springVariants (Operational :: nextRemainingSprings) remainingGroups currentGroupLength) + 
                (springVariants (Damaged :: nextRemainingSprings) remainingGroups currentGroupLength)

    input
    |> List.map (fun (springs, groups) -> springVariants springs groups 0)
    |> List.sum

let Part2 input = 
    0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 6981
    //let part2Result = Part2 input
    //printfn "Part 2 Result: %d" part2Result // 
    0