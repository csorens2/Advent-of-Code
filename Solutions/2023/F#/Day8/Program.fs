module Day8

open System
open System.IO
open System.Text.RegularExpressions

type Instruction = 
    | Left
    | Right

type TraversalData = {
    /// What node we started on
    StartingNode: string
    /// What node does the loop start on
    LoopStartNode: string option
    /// What instruction does the loop start on
    LoopStartInstruction: int option
    /// On what step do we enter the loop
    LoopStartSteps: int option
    /// How big is the loop
    LoopLength: int option
    /// The number of steps to reach each ending node
    EndingNodesSteps: (string*int) list
}

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

let GetTraversalData (instructions: seq<Instruction>) (nodeMap: Map<string, (string*string)>) startingNode (terminating) = 
    let instructionsArray = Seq.toArray instructions
    let rec recGetTraversalData (traversalData: TraversalData) (visitedNodeInstructionsMap: Map<(string*int), int>) (currInstruction: int) (stepCount: int) (currNode: string) = 
        let nodeInstruction = (currNode, currInstruction)
        if Map.containsKey nodeInstruction visitedNodeInstructionsMap then
            {traversalData with 
                LoopStartNode = Some currNode
                LoopStartInstruction = Some currInstruction
                LoopStartSteps = Some visitedNodeInstructionsMap[nodeInstruction]
                LoopLength = Some (stepCount - visitedNodeInstructionsMap[nodeInstruction])}
        else
            let nextTraversalData = 
                let endingNodeRegex = Regex(@"[\w]+Z")
                match endingNodeRegex.IsMatch currNode with 
                | true -> {traversalData with EndingNodesSteps = (currNode,stepCount) :: traversalData.EndingNodesSteps}
                | false -> traversalData
            let nextNode = 
                let (nextLeftNode, nextRightNode) = nodeMap[currNode]
                match instructionsArray[currInstruction] with 
                | Instruction.Left -> nextLeftNode
                | Instruction.Right -> nextRightNode
            recGetTraversalData 
                nextTraversalData 
                (Map.add nodeInstruction stepCount visitedNodeInstructionsMap) 
                ((currInstruction + 1) % instructionsArray.Length)
                (stepCount + 1) 
                nextNode

    recGetTraversalData 
        {
            TraversalData.StartingNode = startingNode
            LoopStartNode = None
            LoopStartInstruction = None
            LoopStartSteps = None
            LoopLength = None
            EndingNodesSteps = List.empty
        }
        Map.empty
        0
        0
        startingNode

let Part1 input = 
    let (instructions, nodeMap) = input
    
    let instructionsArray = Seq.toArray instructions

    let rec followInstructions currSteps currNode = 
        match currNode = "ZZZ" with 
        | true -> currSteps
        | false ->
            match Map.tryFind currNode nodeMap with 
            | None -> failwith $"Node '{currNode}' not in map."
            | Some (leftNextNode, rightNextNode) -> 
                let nextNode = 
                    match instructionsArray[currSteps % instructionsArray.Length] with 
                    | Instruction.Left -> leftNextNode
                    | Instruction.Right -> rightNextNode
                followInstructions (currSteps + 1) nextNode
    
    followInstructions 0 "AAA"

let Part2 input = 
    let (instructions, (nodeMap: Map<string, (string*string)>)) = input

    let starterNodes = 
        Map.keys nodeMap
        |> Seq.filter (fun nodeName -> Regex(@"[\w]+A").IsMatch nodeName)
        |> Seq.toList
        |> List.map (fun nodeName -> GetTraversalData instructions nodeMap)

    0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 11911
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 
    0