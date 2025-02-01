module Day3

open System.IO
open System.Text.RegularExpressions

type Command = 
    | Do
    | Dont
    | Mul of val1: int * val2: int

let ParseInput filepath = 
    let parseMatch (toParse: Match) = 
        let matchString = toParse.Value
        if matchString.Contains "mul" then 
            Mul(int toParse.Groups.[1].Value, int toParse.Groups.[2].Value)
        else if matchString.Contains "don't" then 
            Dont
        else
            Do        

    let parseLine line = 
        (Regex("mul\\((\\d+),(\\d+)\\)|don't\\(\\)|do\\(\\)").Matches line)
        |> Seq.map parseMatch
        |> Seq.toList

    File.ReadLines(filepath)
    |> Seq.collect parseLine
    |> Seq.toList

let Part1 input = 
    let commandValue command =
        match command with 
        | Do -> 0
        | Dont -> 0
        | Mul(val1, val2) -> val1 * val2
    
    input
    |> List.map commandValue
    |> List.sum

let Part2 input = 
    let rec processCommands (remainingCommands: Command list) active = 
        if remainingCommands.IsEmpty then 
            0
        else 
            match remainingCommands.Head with 
            | Do -> processCommands remainingCommands.Tail true
            | Dont -> processCommands remainingCommands.Tail false
            | Mul(val1, val2) when active -> (val1 * val2) + processCommands remainingCommands.Tail active
            | Mul(_, _) -> processCommands remainingCommands.Tail active
    
    processCommands input true