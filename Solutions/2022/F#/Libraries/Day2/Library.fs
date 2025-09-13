module Day2

open System.Text.RegularExpressions
open System.IO
open Microsoft.FSharp.Reflection

type Shape = 
    | Rock
    | Paper
    | Scissors

type Result = 
    | Lose
    | Win
    | Draw

let ResultToPoints = Map[ (Lose, 0); (Draw, 3); (Win, 6) ]
let ShapeToPoints = Map[ (Rock, 1); (Paper, 2); (Scissors, 3) ]
let LetterToShape = Map[ ("A", Rock); ("B", Paper); ("C", Scissors); ("X", Rock); ("Y", Paper); ("Z", Scissors)]
let LetterToResult = Map [("X", Lose); ("Y", Draw); ("Z", Win)]

let GameResult opponent player = 
    if opponent = player then 
        Draw
    else
        match player with 
        | Rock when opponent = Paper -> Lose
        | Rock when opponent = Scissors -> Win
        | Paper when opponent = Rock -> Win
        | Paper when opponent = Scissors -> Lose
        | Scissors when opponent = Rock -> Lose
        | Scissors when opponent = Paper -> Win
        | _ -> failwith "Unknown game combo"


let ParseInput filepath = 
    let parseLine line = 
        let lineRegex = Regex(@"(\w) (\w)")
        let lineMatch = lineRegex.Match line
        (lineMatch.Groups[1].Value, lineMatch.Groups[2].Value)
        
    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.toList

let Part1 (input: (string * string) list) =             
    input
    |> List.map (fun (opponent, player) -> (LetterToShape[opponent], LetterToShape[player]))
    |> List.map (fun (opponent, player) -> ShapeToPoints[player] + ResultToPoints[(GameResult opponent player)])
    |> List.sum

let Part2 (input: (string * string) list) = 
    let getPlayerChoice opponentChoice expectedResult = 
        FSharpType.GetUnionCases(typeof<Shape>)
        |> Array.toList
        |> List.map (fun case -> FSharpValue.MakeUnion(case, [||]) :?> Shape)
        |> List.filter (fun playerShape -> (GameResult opponentChoice playerShape) = expectedResult)
        |> List.head

    input
    |> List.map (fun (opponentChoice, expectedResult) -> (LetterToShape[opponentChoice], LetterToResult[expectedResult]))
    |> List.map (fun (opponentChoice, expectedResult) -> (getPlayerChoice opponentChoice expectedResult, expectedResult))
    |> List.map (fun (playerChoice, result) -> ShapeToPoints[playerChoice] + ResultToPoints[result])
    |> List.sum


        

