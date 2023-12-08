module Day5

open System
open System.IO
open System.Text.RegularExpressions

type MapEntry = {
    SourceStart: int64
    Range: int64
    DestinationStart: int64
}

type AlmanacEntry = {
    Source: string
    Destination: string
    MapEntries: MapEntry list
}

type SeedRange = {
    Start: int64
    Range: int64
}

let ParseInput filepath = 
    let sourceDestinationRegex = Regex(@"(\w+)-to-(\w+)")
    let rec splitAlmanacEntryLines remainingLines = seq {
        if not (List.isEmpty remainingLines) then
            let almanacEntry = List.takeWhile (fun line -> not (String.IsNullOrEmpty line)) remainingLines
            yield almanacEntry
            let nextRemainingLines = 
                remainingLines
                |> List.skip 1
                |> List.skipWhile (fun line -> not (sourceDestinationRegex.IsMatch line))
            yield! splitAlmanacEntryLines nextRemainingLines
    }
    let buildAlmanacEntry lines = 
        let sourceDestinationMatch = sourceDestinationRegex.Match (List.item 0 lines)
        let mapEntryRegex = Regex(@"(\d+) (\d+) (\d+)")
        let mapEntries = 
            lines
            |> List.skip 1
            |> List.takeWhile (fun line -> mapEntryRegex.IsMatch line)
            |> List.map (fun line ->
                let mapEntryMatch = mapEntryRegex.Match line
                {
                    MapEntry.DestinationStart = int64 mapEntryMatch.Groups.[1].Value
                    SourceStart = int64 mapEntryMatch.Groups.[2].Value
                    Range = int64 mapEntryMatch.Groups.[3].Value
                })
        {
            AlmanacEntry.Source = sourceDestinationMatch.Groups.[1].Value
            Destination = sourceDestinationMatch.Groups.[2].Value
            MapEntries = mapEntries
        }

    let lines = Seq.toList (File.ReadLines(filepath))
    let seeds = 
        Regex(@"(\d+)").Matches lines[0] 
        |> Seq.map (fun seedMatch -> int64 seedMatch.Groups.[1].Value)
        |> Seq.toList
    
    let almanacEntryLines = List.skipWhile (fun (line:string) -> not (sourceDestinationRegex.IsMatch line)) lines

    let almanacMap = 
        splitAlmanacEntryLines almanacEntryLines
        |> Seq.map buildAlmanacEntry
        |> Seq.fold (fun almanacAcc nextAlmanacEntry -> 
            Map.add nextAlmanacEntry.Source nextAlmanacEntry almanacAcc) Map.empty

    (seeds, almanacMap)


let RangesOverlap (start1, range1) (start2, range2) = 
    if range1 = 0L || range2 = 0L then
        failwith "Comparing ranges of length 0"
    else
        let shiftedRange = 
            if start1 < start2 then
                range1 - (start2 - start1)
            else
                range2 - (start1 - start2)
        if shiftedRange > 0L then
            true
        else 
            false
        
let MapSeedRange mapEntries toMap = 
    let rec recGetSeedRanges remainingSeed = seq {
        if remainingSeed.Range > 0 then
            match List.tryFind (fun mapEntry -> 
                RangesOverlap (remainingSeed.Start, remainingSeed.Range) (mapEntry.SourceStart, mapEntry.Range)) mapEntries with
            | None -> yield remainingSeed
            | Some mapEntry ->
                // The trick is: we only destination shift the SeedRange when the everything starting from the Seed's start is covered by the map
                // When there is a piece of the range on the left that is not covered, we split that off, the recurse further. 
                match mapEntry.SourceStart <= remainingSeed.Start with
                | true -> 
                    let startDiff = remainingSeed.Start - mapEntry.SourceStart
                    let shiftedMapRange = mapEntry.Range - startDiff

                    let leftSeedRange = min remainingSeed.Range shiftedMapRange
                    let leftSeed = {SeedRange.Start = mapEntry.DestinationStart + startDiff; SeedRange.Range = leftSeedRange}
                    let rightSeed = {SeedRange.Start = remainingSeed.Start + shiftedMapRange; Range = remainingSeed.Range - leftSeedRange}

                    yield leftSeed
                    yield! recGetSeedRanges rightSeed
                | false -> 
                    let startDiff = mapEntry.SourceStart - remainingSeed.Start
                    let leftSeed = {SeedRange.Start = remainingSeed.Start; Range = startDiff}
                    let rightSeed = {SeedRange.Start = mapEntry.SourceStart; Range = remainingSeed.Range - startDiff}
                    let test = rightSeed
                    
                    yield! recGetSeedRanges leftSeed
                    yield! recGetSeedRanges rightSeed            
    }
    
    recGetSeedRanges toMap
    |> Seq.toList


let GetMinimumSeed almanacMap initialCatagory seedRanges = 
    let rec resolveChain currCatagory seedRanges = 
        match Map.tryFind currCatagory almanacMap with
        | None -> seedRanges
        | Some almanacEntry ->
            let nextSeedRanges = 
                seedRanges
                |> List.map (MapSeedRange almanacEntry.MapEntries)
                |> List.collect id
            resolveChain almanacEntry.Destination nextSeedRanges

    let minSeedRange = 
        seedRanges
        |> Seq.toList
        |> resolveChain initialCatagory
        |> List.minBy (fun seedRange -> seedRange.Start)
    minSeedRange.Start

let Part1 input = 
    let (seeds, almanacMap) = input
    // We can unify the solution for the two parts to reframe Part1 as using SeedRanges all of range 1
    seeds
    |> List.map (fun seedNum -> {SeedRange.Start = seedNum; Range = 1L})
    |> GetMinimumSeed almanacMap "seed"

let Part2 input = 
    let rec buildSeedRanges remainingNums = seq {
        if not (List.isEmpty remainingNums) then
            yield {SeedRange.Start = List.item 0 remainingNums; Range = List.item 1 remainingNums}
            yield! buildSeedRanges (List.skip 2 remainingNums)
    } 

    let (seeds, almanacMap) = input
    buildSeedRanges seeds
    |> Seq.toList
    |> GetMinimumSeed almanacMap "seed"

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 227653707
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 78775051
    0