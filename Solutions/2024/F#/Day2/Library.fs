module Day2

open System.IO
open System.Text.RegularExpressions

type Trend = 
    | Increasing
    | Decreasing

let ParseInput filepath = 
    let parseLine line = 
        Regex(@"(\d+)").Matches(line)
        |> Seq.map (fun lineMatch -> int lineMatch.Groups.[1].Value)
        |> Seq.toList

    File.ReadLines(filepath)
    |> Seq.toList
    |> List.map parseLine

let rec IsSafe (remainingValues: int list) (prev: Option<int>) numTrend usedTolerate = 
    if remainingValues.IsEmpty then 
        true
    else if not usedTolerate && (IsSafe remainingValues.Tail prev numTrend true) then 
        true
    else if prev.IsNone then 
        IsSafe remainingValues.Tail (Some(remainingValues.Head)) None usedTolerate
    else if numTrend.IsNone then 
        if prev.Value < remainingValues.Head then 
            IsSafe remainingValues prev (Some(Trend.Increasing)) usedTolerate
        else if prev.Value > remainingValues.Head then 
            IsSafe remainingValues prev (Some(Trend.Decreasing)) usedTolerate 
        else
            false
    else
        let nextVal = remainingValues.Head
        let diff = abs (prev.Value - nextVal)
        if prev.Value < nextVal && numTrend.Value = Decreasing then 
            false
        else if prev.Value > nextVal && numTrend.Value = Increasing then 
            false
        else if diff = 0 || diff > 3 then 
            false
        else
            IsSafe remainingValues.Tail (Some(nextVal)) numTrend usedTolerate

let Part1 (input: int list list) = 
    input
    |> List.map (fun nums -> IsSafe nums None None true)
    |> List.filter id
    |> List.length

let Part2 input = 
    input
    |> List.map (fun nums -> IsSafe nums None None false)
    |> List.filter id
    |> List.length