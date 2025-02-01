module Day1

open System.IO
open System.Text.RegularExpressions

let ParseInput filepath = 
    let lines = 
        File.ReadLines(filepath)
        |> Seq.toList

    let rec parseLines (remainingLines: string list) leftList rightList = 
        if remainingLines.IsEmpty then 
            (leftList, rightList)
        else
            let nums = Regex(@"(\d+)[ ]+(\d+)").Match(remainingLines.Head)
            let leftNum = int nums.Groups.[1].Value
            let rightNum = int nums.Groups.[2].Value
            parseLines (remainingLines.Tail) (leftNum :: leftList) (rightNum :: rightList)

    parseLines lines List.empty List.empty

let Part1 input = 
    let (leftList, rightList) = input
    let leftListSorted = 
        leftList 
        |> List.sort
    let rightListSorted = 
        rightList
        |> List.sort

    let zippedList =
        leftListSorted
        |> List.zip rightListSorted

    zippedList
    |> List.map (fun (leftNum, rightNum) -> abs (leftNum - rightNum))
    |> List.sum

let Part2 input =
    let (leftList, rightList) = input

    let numMapFolder (currMap: Map<int, int>) nextNum = 
        match currMap.TryFind nextNum with 
        | Some(numValue) -> Map.add nextNum (numValue + 1) currMap
        | None -> Map.add nextNum 1 currMap

    let rightMap = List.fold numMapFolder Map.empty rightList

    let getSimilarityScore (rightMap: Map<int, int>) leftNum  = 
        match rightMap.TryFind leftNum with 
        | Some(rightValue) -> leftNum * rightValue
        | None -> 0

    leftList
    |> List.map (getSimilarityScore rightMap)
    |> List.sum