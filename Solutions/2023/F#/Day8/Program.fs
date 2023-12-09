module Day8

open System.IO
open System.Text.RegularExpressions

type Instruction = 
    | Left
    | Right

let ParseInput filepath = 
    let lines = File.ReadLines(filepath)

    let instructionLineRegex = Regex(@"^[RL]+$")
    let instructions = 
        match Seq.tryFind (fun (line: string) -> instructionLineRegex.IsMatch line) lines with 
        | None -> failwith "Instructions line not found"
        | Some instructionsLine -> 
            instructionsLine
            |> Seq.map (fun instructionChar ->
                match instructionChar with 
                | 'L' -> Instruction.Left
                | 'R' -> Instruction.Right
                | unknownInstruction -> failwith $"Unknown instruction: {unknownInstruction}")

    let nodeRegex = Regex(@"^(\w+) = \((\w+), (\w+)\)$")
    let nodeMap = 
        lines
        |> Seq.filter (fun line -> nodeRegex.IsMatch line)
        |> Seq.fold (fun mapAcc line ->
            let nodeMatch = nodeRegex.Match line
            Map.add nodeMatch.Groups.[1].Value (nodeMatch.Groups.[2].Value, nodeMatch.Groups.[3].Value) mapAcc) Map.empty
    (instructions, nodeMap)

let Part1 input = 
    let (instructions, nodeMap) = input

    let rec followInstructions instructionSeq currSteps currNode = 
        match currNode = "ZZZ" with 
        | true -> currSteps
        | false ->
            match Map.tryFind currNode nodeMap with 
            | None -> failwith $"Node '{currNode}' not in map."
            | Some (leftNextNode, rightNextNode) -> 
                let followInstructionsPartial = followInstructions (Seq.skip 1 instructionSeq) (currSteps + 1)
                match Seq.head instructionSeq with 
                | Instruction.Left -> followInstructionsPartial leftNextNode
                | Instruction.Right -> followInstructionsPartial rightNextNode

    let rec instructionLoop (instructionArray: Instruction array) arrayPos = seq {
        yield instructionArray[arrayPos]
        yield! instructionLoop instructionArray ((arrayPos + 1) % instructionArray.Length)
    }

    followInstructions (instructionLoop (Seq.toArray instructions) 0) 0 "AAA"


let Part2 input = 
    0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 
    //let part2Result = Part2 input
    //printfn "Part 2 Result: %d" part2Result // 
    0