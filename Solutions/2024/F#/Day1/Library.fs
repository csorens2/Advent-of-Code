module Day1

open System.IO
open System.Text.RegularExpressions

let ParseInput filepath = 
    let lines = 
        File.ReadLines(filepath)
        |> Seq.toList

    let rec parseLines remainingLines leftList rightList = 
        if List.isEmpty remainingLines then 
            (leftList, rightList)
        else
            let nums = Regex(@"(\d+)[ ]+(\d+)").Match(remainingLines.Head)
            let leftNum = int nums.Groups.[1].Value
            let rightNum = int nums.Groups.[2].Value
            parseLines (remainingLines.Tail) (leftNum :: leftList) (rightNum :: rightList)

    parseLines lines List.empty List.empty

let Part1 input = 
    let (leftList, rightList) = input

    let leftListSorted = List.sort leftList
    let rightListSorted = List.sort rightList

    let zippedList = List.zip leftListSorted rightListSorted

    zippedList
    |> List.map (fun (leftNum, rightNum) -> abs (leftNum - rightNum))
    |> List.sum

let Part2 input =
    let (leftList, rightList) = input

    let numMapFolder currMap nextNum = 
        match Map.tryFind nextNum currMap with 
        | Some(numValue) -> Map.add nextNum (numValue + 1) currMap
        | None -> Map.add nextNum 1 currMap

    let rightMap = List.fold numMapFolder Map.empty rightList

    let getSimilarityScore rightMap leftNum  = 
        match Map.tryFind  leftNum rightMap with 
        | Some(rightValue) -> leftNum * rightValue
        | None -> 0

    leftList
    |> List.map (getSimilarityScore rightMap)
    |> List.sum