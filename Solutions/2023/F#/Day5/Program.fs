module Day5

open System
open System.IO
open System.Text.RegularExpressions

type MapEntry = {
    SourceStart: int64
    DestinationStart: int64
    Range: int64
}

type AlmanacEntry = {
    Source: string
    Destination: string
    MapEntries: MapEntry list
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
    let extractNextAlmanacEntry lines = 
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
        |> Seq.map extractNextAlmanacEntry
        |> Seq.fold (fun (almanacAcc: Map<string,AlmanacEntry>) nextAlmanacEntry -> 
            Map.add nextAlmanacEntry.Source nextAlmanacEntry almanacAcc) Map.empty

    (seeds, almanacMap)

let MapNumber almanacEntry num = 
    let matchEntry mapEntry num = 
        let source = mapEntry.SourceStart
        source <= num && num <= (source + mapEntry.Range)
    let mapNumber mapEntry num = 
        if matchEntry mapEntry num then
            let diff = num - mapEntry.SourceStart
            mapEntry.DestinationStart + diff
        else
            num
    match List.tryFind (fun mapEntry -> matchEntry mapEntry num) almanacEntry.MapEntries with
    | None -> num
    | Some mapEntry -> mapNumber mapEntry num

let Part1 input = 
    let rec resolveChain almanacMap currCategory currNum = 
        match Map.tryFind currCategory almanacMap with 
        | None -> currNum
        | Some almanacEntry -> resolveChain almanacMap almanacEntry.Destination (MapNumber almanacEntry currNum)

    let (seeds, almanacMap) = input
    seeds
    |> List.map (fun seedNum -> resolveChain almanacMap "seed" seedNum)
    |> List.min

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 227653707
    //let part2Result = Part2 input
    //printfn "Part 2 Result: %d" part2Result // 
    0