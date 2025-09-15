module Day3

open System.IO

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map (fun line -> Seq.toList line)
    |> Seq.toList

let CharToPriority toConvert = 
    if System.Char.IsLower toConvert then 
        (int toConvert) - 96
    else
        (int toConvert) - 38

let FindCommonElements elementLists = 
    if List.length elementLists = 0 || List.length elementLists = 1 then 
        failwith "Insufficient lists to compare"
    else
        let elementSets = List.map (fun list -> Set.ofList list) elementLists
        let commonElementSet = List.fold (fun acc next -> Set.intersect acc next ) elementSets.Head elementSets.Tail
        Set.toList commonElementSet
    
let Part1 input = 
    let splitList toSplit = 
        let listLength = List.length toSplit
        [List.take (listLength / 2) toSplit] @ [List.skip (listLength / 2) toSplit]

    input
    |> List.map splitList
    |> List.map FindCommonElements
    |> List.map (fun ruckSackCommon -> List.sum (List.map CharToPriority ruckSackCommon))
    |> List.sum
        

let Part2 input = 
    let rec elfGroups remainingElves = seq {
        if not (List.isEmpty remainingElves) then 
            yield List.take 3 remainingElves
            yield! elfGroups (List.skip 3 remainingElves)
    }

    elfGroups input
    |> Seq.toList
    |> List.map FindCommonElements
    |> List.map (fun elfGroup -> List.sum (List.map CharToPriority elfGroup))
    |> List.sum