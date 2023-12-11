module Day9

open System.Text.RegularExpressions
open System.IO

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map (fun line ->
        Regex(@"((\d+)|(-\d+))").Matches line
        |> Seq.map (fun lineMatch -> int lineMatch.Groups.[0].Value))

let rec CalculateNextValue remainingValues diffList = 
    match remainingValues with 
    | [] -> failwith "Unable to calculate the next value on an empty list"
    | [singlevalue] -> 
        let diffSet = Set.ofList diffList
        let diffValue = 
            match Set.count diffSet with 
            | 0 -> failwith "Unable to calculate next value on a list of size 1"
            | 1 -> List.head diffList
            | _ -> 
                // Lists in F# are linked lists with only a head pointer, no tail pointers
                // This makes appending to the end very expensive
                // So to build a list from front to back efficiently, you need to append to the front, and then reverse before using it
                CalculateNextValue (List.rev diffList) (list.Empty)
        singlevalue + diffValue
    | firstValue :: secondVaue :: _ -> 
        CalculateNextValue (List.tail remainingValues) ((secondVaue - firstValue) :: diffList)

let Part1 input = 
    input
    |> Seq.map (fun nums -> CalculateNextValue (Seq.toList nums) List.empty)
    |> Seq.sum

let Part2 input = 
    input 
    |> Seq.map (fun nums ->
        let reversedNums = 
            nums
            |> Seq.toList
            |> Seq.rev
            |> Seq.toList
        CalculateNextValue reversedNums List.empty)
    |> Seq.sum

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 1972648895
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 919
    0