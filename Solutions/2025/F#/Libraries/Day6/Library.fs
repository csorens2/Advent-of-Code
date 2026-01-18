module Day6

open System
open System.IO
open System.Text.RegularExpressions

type Operation = 
    | Add
    | Multiply

let OperationMap = 
    [('+', Add); ('*', Multiply)]
    |> Map.ofList

type Problem = {
    Numbers: int64 list
    Operation: Operation
}

let ParseInput filepath =
    File.ReadLines(filepath)
    |> Seq.toList

let CombineProblems problems = 
    let foldProblem op acc nextNum = 
        match op with 
        | Multiply -> acc * nextNum
        | Add -> acc + nextNum

    problems
    |> List.map (fun nextProblem -> List.reduce (foldProblem nextProblem.Operation) nextProblem.Numbers)
    |> List.sum

let Part1 input = 

    let generateProblemLists (rawInput: string list) = 

        let numLineRegex = Regex("(\d+)")
        let operationLineRegex = Regex("([*|+])")

        let processNumLine line = 
            numLineRegex.Matches(line)
            |> Seq.map (fun numMatch -> int64 numMatch.Groups[1].Value)
            |> Seq.toArray

        let processOperationLine line = 
            line
            |> Seq.filter (fun lineChar -> lineChar <> ' ')
            |> Seq.map (fun linechar -> Map.find linechar OperationMap) 

        let numArrays = 
            rawInput
            |> Seq.filter (fun line -> numLineRegex.IsMatch(line))
            |> Seq.map processNumLine
            |> Seq.toList

        let operationArray = 
            rawInput
            |> Seq.filter (fun line -> operationLineRegex.IsMatch(line))
            |> Seq.map processOperationLine
            |> Seq.head
            |> Seq.toArray

        let rec generateProblems index = seq {
            if index < Array.length operationArray then 
                yield 
                    {
                        Problem.Numbers = List.map (fun numArray -> Array.item index numArray) numArrays
                        Operation = Array.item index operationArray
                    }
                yield! generateProblems (index+1)
        }

        generateProblems 0
        |> Seq.toList
            
    CombineProblems (generateProblemLists input)

let Part2 input = 
    
    let generateProblemLists rawInput = 
        let rawArrays = 
            rawInput
            |> List.map Seq.toArray
            |> List.toArray

        let getColumnChars index = 
            rawArrays
            |> Array.map (fun lineArray -> lineArray[index])

        let rec generateProblems currIndex beginningIndex endIndex = seq {
            if not (Option.isNone beginningIndex && Option.isNone endIndex && currIndex = Array.length (rawArrays[0])) then 
                
                if Option.isNone beginningIndex && Option.isNone endIndex then 
                    let currIndexColumn = getColumnChars currIndex

                    let isStartColumn = Array.exists (fun columnChar -> List.contains columnChar ['*'; '+']) currIndexColumn
                    if isStartColumn then 
                        yield! generateProblems (currIndex + 1) (Some(currIndex)) None
                    else 
                        yield! generateProblems (currIndex + 1) None None

                else if Option.isSome beginningIndex && Option.isNone endIndex then 
                    if currIndex = Array.length (rawArrays[0]) then 
                        yield! generateProblems (currIndex) beginningIndex (Some(currIndex - 1))
                    else
                        let currIndexColumn = getColumnChars currIndex
                        let isEndColumn = Array.forall (fun columnChar -> columnChar = ' ') currIndexColumn
                        if isEndColumn then 
                            yield! generateProblems (currIndex + 1) beginningIndex (Some(currIndex - 1))
                        else
                            yield! generateProblems (currIndex + 1) beginningIndex None

                else if Option.isSome beginningIndex && Option.isSome endIndex then 
                    let operation = 
                        match Array.last (getColumnChars beginningIndex.Value) with 
                        | '*' -> Multiply
                        | '+' -> Add
                        | unknown -> failwith $"Unknown operation '{unknown}'"
                    
                    let foldColumn acc next =
                        if Char.IsNumber next then 
                            acc + (string next)
                        else
                            acc
                        
                    let problemNumbers = 
                        [beginningIndex.Value..endIndex.Value]
                        |> List.map getColumnChars
                        |> List.map (fun columnChars -> Array.fold foldColumn "" columnChars)
                        |> List.map int64

                    yield {Problem.Numbers = problemNumbers; Operation = operation}
                    yield! generateProblems (currIndex) None None
                else 
                    failwith "Somehow have the end-index without the beginning"
        }
        generateProblems 0 None None
    
    CombineProblems (Seq.toList (generateProblemLists input))