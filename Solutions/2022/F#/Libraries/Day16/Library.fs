module Day16

open System.IO
open System.Text.RegularExpressions
open System.Collections.Immutable

type Valve = {
    Name: string
    FlowRate: int
    Connections: string list
}

let ParseInput filepath = 
    let parseLine line = 
        let lineRegex = Regex("""Valve (.+) has flow rate=(\d+); tunnel[s]? lead[s]? to valve[s]? (.+)""")
        let lineMatch = lineRegex.Match line
        {
            Valve.Name = lineMatch.Groups[1].Value; 
            FlowRate = (int lineMatch.Groups[2].Value); 
            Connections = Array.toList (lineMatch.Groups[3].Value.Replace(" ", "").Split(','))
        }

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.map (fun valve -> (valve.Name, valve))
    |> Map.ofSeq

let Part1 input = 
    
    let rec getDistance (bfsQueue: ImmutableQueue<string * Set<string> * int>) target = 
        if bfsQueue.IsEmpty then 
            -1
        else
            let (nextCurr, nextVisited, nextSteps) = bfsQueue.Peek()
            let poppedQueue = bfsQueue.Dequeue()

            if nextCurr = target then 
                nextSteps
            else
                let nextQueue = 
                    (Map.find nextCurr input).Connections
                    |> List.filter (fun possibleNext -> not (Set.contains possibleNext nextVisited))
                    |> List.fold (fun (acc:ImmutableQueue<string * Set<string> * int>) nextValve -> acc.Enqueue((nextValve, Set.add nextValve nextVisited, nextSteps + 1))) poppedQueue
            
                getDistance nextQueue target

    let distanceMap = 
        [
            for source in input.Keys do
                let subMap = 
                    [
                        for destination in input.Keys do
                            if source <> destination && input[destination].FlowRate <> 0 then 
                                yield (destination, getDistance (ImmutableQueue.Empty.Enqueue((source, Set.add source Set.empty, 0))) destination)
                    ]
                    |> Map.ofList
                if source = "AA" || (Map.find source input).FlowRate <> 0 then 
                    yield (source, subMap)
        ]
        |> Map.ofList

    let rec findMaxFlow curr openedValves currFlow remainingTime = 
        if remainingTime = 0 then 
            0
        else
            if not (Set.contains curr openedValves) then 
                currFlow + findMaxFlow curr (Set.add curr openedValves) (currFlow + (Map.find curr input).FlowRate) (remainingTime - 1)
            else
                let mapValves nextPossibleValve = 
                    let travelTime = distanceMap[curr][nextPossibleValve]
                    (currFlow * travelTime) + findMaxFlow nextPossibleValve openedValves currFlow (remainingTime - travelTime)

                distanceMap[curr]
                |> Map.keys
                |> Seq.filter (fun nextPossibleValve -> not (Set.contains nextPossibleValve openedValves))
                |> Seq.filter (fun nextPossibleValve -> distanceMap[curr][nextPossibleValve] < remainingTime)
                |> Seq.map mapValves
                |> Seq.append (Seq.singleton (currFlow * remainingTime))
                |> Seq.max
    


    findMaxFlow "AA" (Set.add "AA" Set.empty) 0 30




let Part2 input = 
    0