module Day4

open System.IO
open System.Text.RegularExpressions


let ParseInput filepath =
    let parseLine line = 
        let lineRegex = Regex(@"((\d+)-(\d+)),((\d+)-(\d+))")
        let lineMatch = lineRegex.Match line
        ((int (lineMatch.Groups[2].Value), int (lineMatch.Groups[3].Value)), (int (lineMatch.Groups[5].Value), int (lineMatch.Groups[6].Value)))

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.toList

let Contains (leftOuter, rightOuter) (leftInner, rightInner) = 
    if leftOuter <= leftInner && rightInner <= rightOuter then 
        true
    else
        false

let Overlaps (left1, right1) (left2, right2) = 
    if left1 <= left2 && left2 <= right1 then 
        true
    else if left1 <= right2 && right2 <= right1 then 
        true
    else 
        false

let Part1 input =
    input
    |> List.filter (fun (left, right) -> (Contains left right) || (Contains right left))
    |> List.length

let Part2 input = 
    input
    |> List.filter (fun (left, right) -> (Overlaps left right) || (Overlaps right left))
    |> List.length