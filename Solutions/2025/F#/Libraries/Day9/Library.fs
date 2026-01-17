module Day9

open System.IO
open System.Text.RegularExpressions

let ParseInput filepath = 

    let parseLine line = 
        let numPairRegex = Regex("(\d+),(\d+)")
        (int64 (numPairRegex.Match(line).Groups.[1].Value), int64 (numPairRegex.Match(line).Groups.[2].Value))

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.toList

let Part1 (input: (int64*int64) list) = 

    let inputArray = List.toArray input

    let processPointPair (point1,point2) = 
        let (y1, x1) = point1
        let (y2, x2) = point2
        (abs (x2 - x1) + 1L) * (abs (y2 - y1) + 1L)

    let pointPairs = seq {
        for i = 0 to Array.length inputArray - 1 do 
            for j = i+1 to Array.length inputArray - 1 do 
                yield (inputArray[i], inputArray[j])
    }
    
    pointPairs
    |> Seq.map processPointPair
    |> Seq.max

let Part2 input = 
    0