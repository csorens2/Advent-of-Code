﻿open System.IO

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.toList

[<EntryPoint>]
let main _ =
    printfn "Hello from F#"

    let input = ParseInput("Input.txt")
    // let part1Result = Part1
    // printfn "Part 1 Result: %d" part1Result 
    // let part2Result = Part2
    // printfn "Part 2 Result: %d" part2Result
    0