open System
open System.IO
open System.Text.RegularExpressions

type Instruction = 
    | acc = 0
    | jmp = 1
    | nop = 2

let InstructionPosAccDict = 
    dict [
        (Instruction.acc, (fun instPos instVal acc -> (instPos + 1, acc + instVal)));
        (Instruction.jmp, (fun instPos instVal acc -> (instPos + instVal, acc)));
        (Instruction.nop, (fun instPos _ acc -> (instPos + 1, acc)));
    ]

let ParseInput filepath = 
    let lineRegex = Regex(@"(\w+)([\s]?(.*))?", RegexOptions.Compiled)    
    File.ReadLines(filepath)
    |> Seq.map (fun x -> lineRegex.Match(x))
    |> Seq.map (fun x -> (Enum.Parse<Instruction>(x.Groups[1].Value), int(x.Groups[3].Value)))
    |> Seq.toList

let Part1 (instList:(Instruction * int) list) = 
    let rec processInst (visitedSet:Set<int>) instPos acc = 
        if visitedSet.Contains instPos then
            acc
        else
            let (inst, instVal) = instList[instPos]
            let nextSet = visitedSet.Add(instPos)
            let (nextPos, nextAcc) = InstructionPosAccDict[inst] instPos instVal acc
            processInst nextSet nextPos nextAcc
    processInst Set.empty 0 0

let Part2 (instList:(Instruction * int) list) = 
    let rec processInst (visitedSet:Set<int>) instPos acc usedSwap = 
        if instList.Length <= instPos then
            (acc, true)
        elif visitedSet.Contains instPos then
            (acc, false)
        else
            let (inst, instVal) = instList[instPos]
            let nextSet = visitedSet.Add(instPos)
            let (nextPos, nextAcc) = InstructionPosAccDict[inst] instPos instVal acc
            let (childAcc, halted) = processInst nextSet nextPos nextAcc usedSwap
            if halted then
                (childAcc, halted)
            elif (inst = Instruction.jmp || inst = Instruction.nop) && not usedSwap then
                let instFlipDict = 
                    dict [
                        (Instruction.jmp, Instruction.nop);
                        (Instruction.nop, Instruction.jmp)
                    ]
                let replacementInst = instFlipDict[inst]
                let (replacementNextPos, replacementNextAcc) = InstructionPosAccDict[replacementInst] instPos instVal acc
                processInst nextSet replacementNextPos replacementNextAcc true
            else
                (childAcc, false)                        
    fst (processInst Set.empty 0 0 false)

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 1475
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 1270
    0