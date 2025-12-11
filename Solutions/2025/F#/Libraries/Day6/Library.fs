module Day6

open System.IO
open System.Text.RegularExpressions

type Operation = 
    | Add
    | Multiply

type Problem = {
    Numbers: int64 list
    Operation: Operation
}

let ParseInput filepath = 

    let numLineRegex = Regex("(\d+)")
    let operationLineRegex = Regex("([*|+])")

    let processNumLine line = 
        numLineRegex.Matches(line)
        |> Seq.map (fun numMatch -> int64 numMatch.Groups[1].Value)
        |> Seq.toArray

    let processOperationLine line = 
        line
        |> Seq.filter (fun lineChar -> lineChar <> ' ')
        |> Seq.map (fun linechar -> if linechar = '*' then Multiply else Add) 


    let lines = File.ReadLines(filepath)

    let numArrays = 
        lines
        |> Seq.filter (fun line -> numLineRegex.IsMatch(line))
        |> Seq.map processNumLine
        |> Seq.toList

    let operationArray = 
        lines
        |> Seq.filter (fun line -> operationLineRegex.IsMatch(line))
        |> Seq.map processOperationLine
        |> Seq.head
        |> Seq.toArray

    let rec generateProblems i = seq {
        if i < Array.length operationArray then 
            yield 
                {
                    Problem.Numbers = List.map (fun numArray -> Array.item i numArray) numArrays
                    Operation = Array.item i operationArray
                }
            yield! generateProblems (i+1)
    }

    generateProblems 0
    |> Seq.toList

let Part1 input = 

    let foldProblem op acc nextNum = 
        match op with 
        | Multiply -> acc * nextNum
        | Add -> acc + nextNum

    input
    |> List.map (fun nextProblem -> List.reduce (foldProblem nextProblem.Operation) nextProblem.Numbers)
    |> List.sum

let Part2 input = 
    
    let mapNums (nums: int64 list) = 
        
        0
    
    0