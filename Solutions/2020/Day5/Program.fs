open System.IO
open System.Text.RegularExpressions

let planeWidth = 8
let planeLength = 128

let ticketID (row,col) = 
    (row * 8) + col

let ParseInput filepath = 
    let rec processRowCol min max (chars:seq<char>) = 
        let nextChar = Seq.head(chars)
        let moveDelta = double(max - min) / 2.0
        let nextMin = 
            if nextChar = 'F' || nextChar = 'L' then
                min
            else
                int(ceil (double(min) + moveDelta))
        let nextMax = 
            if nextChar = 'B' || nextChar = 'R' then
                max
            else
                int(floor (double(max) - moveDelta))
        if Seq.isEmpty(Seq.tail(chars)) then
            if nextChar = 'F' || nextChar = 'L' then
                nextMin
            else
                nextMax
        else
            processRowCol nextMin nextMax (Seq.tail(chars))
    let rowColRegex = Regex(@"([BF]*)([RL]*)", RegexOptions.Compiled)
    File.ReadLines(filepath)
    |> Seq.map (fun x -> (rowColRegex.Match(x).Groups[1].Value, rowColRegex.Match(x).Groups[2].Value))
    |> Seq.map (fun (row, col) -> ((processRowCol 0 (planeLength - 1) row), (processRowCol 0 (planeWidth - 1) col)))

let Part1 (input:seq<int*int>) =
    input
    |> Seq.map ticketID
    |> Seq.max

let Part2 (input:seq<int*int>) = 
    let unfilledAirplane = 
        [0..(planeLength - 1)]
        |> Seq.fold (fun (acc:Map<int,Set<int>>) next -> acc.Add(next, Set.empty)) Map.empty
    let possibleRows = 
        input
        |> Seq.fold (fun (acc:Map<int,Set<int>>) (nextRow, nextCol) -> acc.Add(nextRow, acc[nextRow].Add(nextCol))) unfilledAirplane
        |> Seq.filter (fun x -> x.Value.Count <> 0)
        |> Seq.toList
    let targetRow = 
        possibleRows
        |> List.removeAt(possibleRows.Length - 1)
        |> List.skip 1
        |> List.filter (fun x -> x.Value.Count <> 8)
        |> List.head
    let emptyCol = 
        [0..(planeWidth - 1)]
        |> Seq.filter (fun x -> not (targetRow.Value.Contains(x)))
        |> Seq.head
    ticketID (targetRow.Key, emptyCol)

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 892
    let part2Result = Part2(input)
    printfn "Part 2 Result: %d" part2Result // 625
    0