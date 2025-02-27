module Day11

open System.IO
open System.Text.RegularExpressions

let ParseInput filepath = 
    let parseLine (line: string) = 
        Regex("(\d+)").Matches line
        |> Seq.map (fun nextMatch -> nextMatch.Groups[0].Value)

    File.ReadLines(filepath)
    |> Seq.head
    |> parseLine

let rec processBlink (remainingList: string list) = seq {
    if not remainingList.IsEmpty then 
        let nextNum = remainingList.Head
        if nextNum = "0" then 
            yield "1"
        else if nextNum.Length % 2 = 0 then 
            let leftNum = int64 (nextNum.Substring(0, nextNum.Length / 2))
            let rightNum = int64 (nextNum.Substring(nextNum.Length / 2))
            yield leftNum.ToString()
            yield rightNum.ToString()
        else 
            yield ((int64 nextNum) * 2024L).ToString()
        yield! processBlink (remainingList.Tail)
}
let rec blink (numBlink: int) (numList: string list)  =
    if numBlink = 0 then 
        numList
    else
        let nextBlink = 
            processBlink numList
            |> Seq.toList
        blink (numBlink - 1) nextBlink 


let Part1 (input: string seq) = 
    Seq.toList input
    |> blink 25
    |> List.length


let Part2 input = 
    Seq.toList input
    |> blink 75
    |> List.length