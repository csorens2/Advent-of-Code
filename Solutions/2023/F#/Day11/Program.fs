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

    // First, we get which rows need to be expanded
    let rowLength = List.length input
    let colLength = List.length (List.item 0 input)

    let inputArray = 
        input
        |> List.toArray
        |> Array.map (fun row -> List.toArray row)

    let rec getEmptyRowsCols (remainingRows, remainingCols)


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