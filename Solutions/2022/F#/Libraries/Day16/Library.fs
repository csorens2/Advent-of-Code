module Day16

open System.IO
open System.Text.RegularExpressions

type Valve = {
    FlowRate: int
    Connections: int list
}

let ParseInput filepath = 
    
    let parseLine line = 
        let lineRegex = Regex("""Valve (.+) has flow rate=(\d+); tunnel[s]? lead[s]? to valve[s]? (.+)""")
        let lineMatch = lineRegex.Match line
        (
            lineMatch.Groups[1].Value,
            (int lineMatch.Groups[2].Value),
            Array.toList (lineMatch.Groups[3].Value.Replace(" ", "").Split(','))
        )

    let parsedLines = 
        File.ReadLines(filepath)
        |> Seq.map parseLine

    let valveNameToNumMap = 
        parsedLines
        |> Seq.map (fun (name, _, _) -> name)
        |> Seq.sort
        |> Seq.indexed
        |> Seq.map (fun (num, name) -> (name, num))
        |> Map.ofSeq
        
    parsedLines
    |> Seq.map (fun (name, flow, connections) -> 
        (
            valveNameToNumMap[name],
            {
                Valve.FlowRate = flow;
                Connections = List.map (fun connection -> valveNameToNumMap[connection]) connections
            }
        ))
    |> Map.ofSeq

let GeneratePathsAndFlow input time = 
    
    let startNode = 0

    let matrixLength = (Seq.length (Map.keys input))

    let maxValue = System.Int32.MaxValue

    let distanceMatrix = 
        Array.init matrixLength (fun y -> 
            Array.init matrixLength (fun x -> 
                if y = x then 
                    0
                else if (Map.containsKey y input) && (List.contains x input[y].Connections) then 
                    1
                else 
                    maxValue
            )
        )

    for intermediate in [0..matrixLength-1] do 
        for source in [0..matrixLength-1] do
            for destination in [0..matrixLength-1] do 
                if distanceMatrix[source][intermediate] <> maxValue && distanceMatrix[intermediate][destination] <> maxValue then 
                    distanceMatrix[source][destination] <- min (distanceMatrix[source][destination]) (distanceMatrix[source][intermediate] + distanceMatrix[intermediate][destination])
    
    let relevantNums = 
        input
        |> Map.toList
        |> List.filter (fun (num, valve) -> valve.FlowRate <> 0 || num = startNode)
        |> List.map (fun (num, _) -> num)

    let rec findPathsAndFlow curr openedValves currFlow totalFlow remainingTime = 
        if remainingTime = 0 then 
            [(openedValves, totalFlow)]
        else
            if not (Set.contains curr openedValves) then 
                findPathsAndFlow curr (Set.add curr openedValves) (currFlow + (Map.find curr input).FlowRate) (totalFlow + currFlow) (remainingTime - 1)
            else
                let mapValves nextPossibleValve = 
                    let travelTime = distanceMatrix[curr][nextPossibleValve]
                    findPathsAndFlow nextPossibleValve openedValves currFlow (totalFlow + (currFlow * travelTime)) (remainingTime - travelTime)
                
                relevantNums
                |> List.filter (fun nextPossibleValve -> not (Set.contains nextPossibleValve openedValves))
                |> List.filter (fun nextPossibleValve -> distanceMatrix[curr][nextPossibleValve] < remainingTime)
                |> List.collect mapValves
                |> List.append [(openedValves, totalFlow + (currFlow * remainingTime))]
                
    findPathsAndFlow startNode (Set.add startNode Set.empty) 0 0 time
    |> List.sortBy (fun (_, flow) -> flow)
    |> List.fold (fun acc (nextSet, nextFlow) -> Map.add nextSet nextFlow acc) Map.empty


let Part1 input = 
    let (_, maxFlow) = 
        GeneratePathsAndFlow input 30
        |> Map.toList
        |> List.sortByDescending (fun (_, flow) -> flow)
        |> List.head

    maxFlow
    

let Part2 input = 
    let pathsAndFlow = 
        GeneratePathsAndFlow input 26
        |> Map.toList
        |> List.sortByDescending (fun (_,flow) -> flow)

    let pathCombos = seq {
        for (setA, flowA) in pathsAndFlow do 
            let setAFixed = Set.remove 0 setA
            for (setB, flowB) in pathsAndFlow do 
                let setBFixed = Set.remove 0 setB
                if Set.isEmpty (Set.intersect setAFixed setBFixed)  then 
                    yield flowA + flowB
    }

    pathCombos
    |> Seq.sortDescending
    |> Seq.head