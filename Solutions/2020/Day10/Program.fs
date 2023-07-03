open System
open System.IO

let maxJoltJump = 3

let ParseInput filepath = 
    let input = 
        File.ReadLines(filepath)
        |> Seq.map int
        |> Seq.toList
    let builtInAdapter = 
        (input |> List.max) + maxJoltJump
    0::builtInAdapter::input
    |> List.sort

let Part1 input = 
    let rec processAdapters joltList oneJoltDiffs threeJoltDiffs = 
        match joltList with
        | [] | [_] -> oneJoltDiffs * threeJoltDiffs
        | current::next::remaining -> 
            match next - current with
            | 1 -> processAdapters (next::remaining) (oneJoltDiffs + 1) threeJoltDiffs
            | 3 -> processAdapters (next::remaining) oneJoltDiffs (threeJoltDiffs + 1)
            | _ -> processAdapters (next::remaining) oneJoltDiffs threeJoltDiffs
    processAdapters input 0 0

let Part2 input = 
    let rec processAdapters adapterMap adapterList = 
        match adapterList with 
        | [] -> raise (Exception "Processing empty adapter list.")
        | [_] -> adapterMap
        | currAdapter::remainingAdapters ->
            match Map.tryFind currAdapter adapterMap with
            | Some _ -> adapterMap
            | None -> 
                let nextAdapters = 
                    remainingAdapters
                    |> List.takeWhile (fun volt -> volt <= currAdapter + uint64(maxJoltJump))
                match List.isEmpty nextAdapters with
                | true -> adapterMap
                | false -> 
                    let updatedAdapterMap = 
                        nextAdapters
                        |> List.fold (fun acc nextAdapter -> 
                            let nextList =   
                                adapterList
                                |> List.skipWhile (fun adapterVolt -> adapterVolt <= nextAdapter)
                            processAdapters acc (nextAdapter::nextList)) adapterMap
                    let adapterPossibilities = 
                        nextAdapters
                        |> List.fold (fun acc nextAdapter -> acc + updatedAdapterMap[nextAdapter]) 0UL
                    updatedAdapterMap.Add(currAdapter, adapterPossibilities)     
    let convertedList = 
        input
        |> List.map uint64
    let lastAdapter = 
        List.last convertedList
    let baseMap = 
        Map.add lastAdapter 1UL Map.empty
    let resultMap = processAdapters baseMap convertedList
    resultMap[0UL]

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt") |> Seq.toList
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 2277
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 37024595836928
    0