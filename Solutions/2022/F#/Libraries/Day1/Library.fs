module Day1

open System.IO

let ParseInput filepath = 
    let rec processLines remainingLines = seq {
        let nextElfLines = 
            remainingLines
            |> List.takeWhile (fun line -> line <> "" ) 
            |> List.map (fun line -> System.Int32.Parse(line))
        yield nextElfLines
        
        let remainingElfLines = 
            remainingLines
            |> List.skipWhile (fun line -> line <> "")
        if List.tryHead remainingElfLines <> None then 
            yield! processLines (List.skip 1 remainingElfLines)
    }
    
    File.ReadLines(filepath)
    |> Seq.toList
    |> processLines
    |> Seq.toList

let Part1 (input: int list list) = 
    input
    |> List.map (fun elfList -> List.sum elfList)
    |> List.sortDescending
    |> List.head

let Part2 (input: int list list) = 
    input
    |> List.map (fun elfList -> List.sum elfList)
    |> List.sortDescending
    |> List.take 3
    |> List.sum