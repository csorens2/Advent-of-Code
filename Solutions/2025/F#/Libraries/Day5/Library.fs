module Day5

open System.IO
open System.Text.RegularExpressions


let ParseInput filepath = 
    let rangeRegex = Regex("((\d+)-(\d+))")
    let idRegex = Regex("^(\d+)$")

    let parseRangeLine line = 
        let regexMatch = rangeRegex.Match(line)
        (int64 regexMatch.Groups[2].Value, int64 regexMatch.Groups[3].Value)

    let lines = File.ReadLines(filepath)

    let ranges = 
        lines
        |> Seq.filter (fun line -> rangeRegex.IsMatch(line))
        |> Seq.map parseRangeLine
        |> Seq.toList

    let ids = 
        lines
        |> Seq.filter (fun line -> idRegex.IsMatch(line))
        |> Seq.map (fun line -> int64 line)
        |> Seq.toList

    (ranges, ids)

let Part1 input =
    let (ranges, ids) = input

    let isFresh id = 
        let foundValidRange = 
            List.tryFind 
                (fun (left, right) -> if left <= id && id <= right then true else false) 
                ranges
        match foundValidRange with 
        | Some (_) -> true
        | None -> false
    
    ids
    |> List.filter isFresh
    |> List.length

let Part2 input = 
    let (ranges, _) = input

    let rec mergeRanges remainingRanges = seq {
        if not (List.isEmpty remainingRanges) then 
            let (firstRangeLeft, firstRangeRight) = List.head remainingRanges

            let partitionFunction (toCheckLeft, toCheckRight) = 
                if firstRangeLeft <= toCheckLeft && toCheckLeft <= firstRangeRight then 
                    true
                else if firstRangeLeft <= toCheckRight && toCheckRight <= firstRangeRight then 
                    true
                else if toCheckLeft <= firstRangeLeft && firstRangeRight <= toCheckRight then 
                    true
                else 
                    false
            let (canMerge, cantMerge) = List.partition partitionFunction (List.tail remainingRanges)

            if List.isEmpty canMerge then 
                yield (firstRangeLeft, firstRangeRight)
                yield! mergeRanges (List.tail remainingRanges)
            else 
                let (mergeLeftRange, _) =  List.minBy (fun (left, _) -> left) canMerge
                let (_, mergeRightRange) = List.maxBy (fun (_, right) -> right) canMerge
                let newLeftRange = min firstRangeLeft mergeLeftRange
                let newRightRange = max firstRangeRight mergeRightRange
                yield! mergeRanges ((newLeftRange, newRightRange)::cantMerge)
    }

    let mergedRanges = Seq.toList (mergeRanges ranges)

    mergedRanges
    |> List.map (fun (left, right) -> right - left + 1L)
    |> List.sum