module Day11

open System.IO
open System.Text.RegularExpressions

type Device = {
    Name: string
    Outputs: string list
}

let ParseInput filepath = 
    
    let parseLine line = 
        let regexMatch = Regex(@"(\w+): (.+)").Match(line)
        let name = regexMatch.Groups[1].Value
        let outputs = Array.toList (regexMatch.Groups[2].Value.Split(' '))
        {Device.Name = name; Outputs = outputs}

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.toList


let Part1 (input: Device list) = 
    // From the problem description, we know the graph is Acyclic
    let inputGraph = 
        input
        |> List.map (fun device -> (device.Name, device.Outputs))
        |> Map.ofList

    let rec buildDeviceMap memoizationMap currNode = 
        if Map.containsKey currNode memoizationMap then 
            memoizationMap
        else
            let foldSubGraphs accMap nextNode = 
                let subGraph = buildDeviceMap accMap nextNode
                let toAdd = Map.find nextNode subGraph

                match Map.tryFind currNode subGraph with 
                | None -> Map.add currNode (toAdd) subGraph
                | Some prevValue -> Map.add currNode (toAdd + prevValue) subGraph

            Map.find currNode inputGraph
            |> List.fold foldSubGraphs memoizationMap


    let root = "you"
    let baseMap = Map.add "out" 1 Map.empty

    buildDeviceMap baseMap root
    |> Map.find root


let Part2 input = 
    0