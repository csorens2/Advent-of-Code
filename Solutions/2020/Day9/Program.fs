open System
open System.IO
open System.Diagnostics

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map(fun x -> uint64(x))

let FindInvalidNumber (input:seq<uint64>) (preambleLength:int) = 
    let rec processXMAS (preamble: uint64 list) (numSeq:seq<uint64>) = 
        let rec validatePreamble (preamble: uint64 list) preamIndex targetNum =
            if preamIndex = (List.length preamble) then
                false
            else
                let preamValue = preamble[preamIndex]
                let subPreamble = (List.removeAt preamIndex preamble)
                if List.contains (targetNum - preamValue) subPreamble then
                    true
                else
                    validatePreamble preamble (preamIndex + 1) targetNum
        if Seq.isEmpty numSeq then
            raise (Exception "No corrupted number found")
        else
            let nextNum = Seq.head numSeq
            if validatePreamble preamble 0 nextNum then
                let nextPreamble = 
                    (preamble
                    |> List.skip 1) @ [nextNum]   
                processXMAS nextPreamble (Seq.tail numSeq)
            else
                nextNum

    let starterPreamble = 
        input
        |> Seq.take preambleLength
        |> Seq.toList
    let starterSeq = 
        input
        |> Seq.skip preambleLength
    processXMAS starterPreamble starterSeq

let Part1 (input:seq<uint64>) (preambleLength:int) = 
    FindInvalidNumber input preambleLength

let Part2 (input:seq<uint64>) (preambleLength:int) = 
    let rec findContigList (currentList: uint64 list) currentSum (remainingNums:seq<uint64>) targetValue = 
        let rec pruneContigList (currentList: uint64 list) currentSum targetValue = 
            if currentSum <= targetValue then
                (currentList, currentSum)
            else
                pruneContigList (List.tail currentList) (currentSum - currentList[0]) targetValue
        if Seq.isEmpty remainingNums then
            raise (Exception "No contiguous set found")
        elif currentSum = targetValue then
            currentList
        else
            let nextNum = Seq.head remainingNums
            let (nextList, nextSum) = pruneContigList (currentList @ [nextNum]) (currentSum + nextNum) targetValue
            findContigList nextList nextSum (Seq.tail remainingNums) targetValue
    let invalidNumber = FindInvalidNumber input preambleLength
    let contigList = findContigList (List.empty) (uint64(0)) input invalidNumber
    (List.max contigList) + (List.min contigList)

let Part2Greedy (input:seq<uint64>) (preambleLength:int) = 
    let rec findContigList (remainingNums:seq<uint64>) targetValue = 
        let numsScan = 
            remainingNums
            |> Seq.scan (fun (_, accSum) next -> (next, accSum + next)) (uint64(0), uint64(0))
            |> Seq.takeWhile (fun (_, sum) -> sum <= targetValue)
            |> Seq.skip 1
            |> Seq.toList
        if (not (List.isEmpty numsScan)) && (snd (List.last numsScan)) = targetValue then
            numsScan
            |> List.map (fun (nums, _) -> nums)
        else
            findContigList (Seq.tail remainingNums) targetValue
    
    let invalidNumber = FindInvalidNumber input preambleLength
    let contigList = findContigList input invalidNumber
    (List.max contigList) + (List.min contigList)

[<EntryPoint>]
let main _ =
    let input = ParseInput "Input.txt" 
    let preambleSize = 25
    let part1Result = Part1 input preambleSize
    printfn "Part 1 Result: %d" part1Result  // 258585477

    let stopWatch1 = Stopwatch.StartNew()
    let part2Result = Part2 input preambleSize
    stopWatch1.Stop()
    printfn "Part 2 Result: %d %d" part2Result stopWatch1.Elapsed.Ticks // 36981213

    let stopWatch2 = Stopwatch.StartNew()
    let part2GreedyResult = Part2Greedy input preambleSize
    stopWatch2.Stop()
    printfn "Part 2 \"Greedy\" Result: %d %d" part2GreedyResult stopWatch2.Elapsed.Ticks

    0
