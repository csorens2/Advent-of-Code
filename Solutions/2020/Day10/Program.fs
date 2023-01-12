open System
open System.IO

let maxJoltJump = 3

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map int

let Part1 (input:int list) = 
    let rec processAdapters (joltList:int list) (lastJolt:int) (joltDiffs:int*int*int) = 
        if joltList.IsEmpty then
            joltDiffs
        else
            let nextJolt = joltList.Head
            let nextDiff = nextJolt - lastJolt
            let (diff1, diff2, diff3) = joltDiffs
            let nextJoltDiff = 
                match nextDiff with
                | 1 -> (diff1 + 1, diff2, diff3)
                | 2 -> (diff1, diff2 + 1, diff3)
                | 3 -> (diff1, diff2, diff3 + 1)
                | _ -> raise (Exception "Unexpected diff size")
            processAdapters (joltList.Tail) nextJolt nextJoltDiff
    let sortedInput = 
        input
        |> List.sort
    let builtInJoltage = (List.last sortedInput) + maxJoltJump
    let (diff1, _, diff3) = processAdapters (sortedInput @ [builtInJoltage]) 0 (0,0,0)
    diff1 * diff3

let Part2 (input:int list) = 
    let rec processAdapters (joltMap:Map<uint64, uint64>) (joltList:uint64 list) (currentJolt:uint64) (targetJolt:uint64) =
        if joltMap.ContainsKey(currentJolt) then
            joltMap
        elif currentJolt > targetJolt then
            joltMap.Add(currentJolt, 0UL)
        else
            let nextPossibleJolts = 
                joltList
                |> List.takeWhile (fun x -> x <= currentJolt + uint64(maxJoltJump))
            let updatedJoltMap =  
                nextPossibleJolts
                |> List.fold (fun acc next -> (processAdapters acc (joltList |> List.skipWhile (fun x -> x <= next)) next targetJolt)) joltMap
            let mapValue = 
                nextPossibleJolts
                |> List.fold (fun acc next -> acc + updatedJoltMap[next]) 0UL
            updatedJoltMap.Add(currentJolt, mapValue)
    let sortedConvertedInput = 
        input
        |> List.map uint64
        |> List.sort
    let builtInJoltage = (List.last sortedConvertedInput) + uint64(maxJoltJump)
    let resultMap = processAdapters (Map.empty.Add(builtInJoltage, 1UL)) (sortedConvertedInput @ [builtInJoltage]) 0UL builtInJoltage
    resultMap[0UL]

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt") |> Seq.toList
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 2277
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 37024595836928
    0