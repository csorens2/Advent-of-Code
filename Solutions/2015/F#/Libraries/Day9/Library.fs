module Day9

open System.IO
open System.Text.RegularExpressions

let ParseInput filepath = 
    let foldTravelMap acc nextLine =  
        let pathRegex = Regex("(\w+) to (\w+) = (\d+)")
        let lineMatch = pathRegex.Match(nextLine)
        let dest1 = lineMatch.Groups[1].Value
        let dest2 = lineMatch.Groups[2].Value
        let dist = int lineMatch.Groups[3].Value

        let addDest fromDest toDest prevMap = 
            match Map.tryFind fromDest prevMap with 
            | None -> Map.add fromDest [(toDest, dist)] prevMap
            | Some(prevEntry) -> Map.add fromDest ((toDest, dist) :: prevEntry) prevMap

        addDest dest1 dest2 acc
        |> addDest dest2 dest1

    File.ReadLines(filepath)
    |> Seq.fold foldTravelMap Map.empty

let processTravel input extremaFunc antiExtremaValue = 
    let totalLocations = 
        Map.keys input
        |> Seq.length

    let rec processMove nextLoc locsSet totalDist = 
        let nextSet = Set.add nextLoc locsSet
        if Set.count nextSet = totalLocations then 
            totalDist
        else
            let possibleDests = 
                input[nextLoc]
                |> List.filter (fun (destName, _) -> not (Set.contains destName locsSet))
            if List.isEmpty possibleDests then 
                antiExtremaValue
            else
                possibleDests
                |> List.map (fun (nextDest, nextDist) -> processMove nextDest nextSet (totalDist + nextDist))
                |> extremaFunc
        
    Map.keys input
    |> Seq.map (fun startLoc -> processMove startLoc Set.empty 0)
    |> Seq.toList
    |> extremaFunc

let Part1 input = 
    processTravel input List.min System.Int32.MaxValue

let Part2 input = 
    processTravel input List.max System.Int32.MinValue