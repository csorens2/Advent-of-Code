module Day11

open System.IO

type Space = 
    | Empty
    | Galaxy

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map (fun line ->
        line
        |> Seq.map (fun nextChar -> 
            match nextChar with 
            | '.' -> Empty
            | '#' -> Galaxy
            | _ -> failwith $"Failed attempting to parse unknown character: '{nextChar}'")
        |> Seq.toList)
    |> Seq.toList

let Part1 (input: Space list list) = 

    let inputArray = 
        input
        |> List.toArray
        |> Array.map (fun row -> List.toArray row)

    // First, we get which rows need to be expanded
    let rowLength = Array.length inputArray
    let colLength = Array.length inputArray.[0]

    let rowList = [0..rowLength - 1]
    let colList = [0..colLength - 1]

    let (emptyRow, emptyCol) = 
        rowList
        |> List.fold (fun (emptyRowAcc, emptyColAcc) yPos -> 
            colList
            |> List.fold (fun (subEmptyRowAcc, subEmptyColAcc) xPos -> 
                match inputArray.[yPos].[xPos] with 
                | Empty -> (subEmptyRowAcc, subEmptyColAcc)
                | Galaxy -> (Set.remove yPos subEmptyRowAcc, Set.remove xPos subEmptyColAcc)) (emptyRowAcc, emptyColAcc)) (Set.ofList rowList, Set.ofList colList)

    0

let Part2 input = 
    0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    //let part1Result = Part1 input
    //printfn "Part 1 Result: %d" part1Result // 
    //let part2Result = Part2 input
    //printfn "Part 2 Result: %d" part2Result // 
    0