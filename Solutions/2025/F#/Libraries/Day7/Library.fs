module Day7

open System.IO

type Space = 
    | Open
    | Start
    | Splitter

let ParseInput filepath = 
    let parseLine line = 
        line
        |> Seq.map (fun lineChar -> if lineChar = 'S' then Start else if lineChar = '.' then Open else Splitter)
        |> Seq.toArray

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.toArray

let Part1 (input: Space array array) = 
    0


let Part2 input = 
    0