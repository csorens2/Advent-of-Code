module Day11

open System.IO
open System.Text.RegularExpressions

let ParseInput filepath = 
    
    let parseLine line = 
        let regexMatch = Regex(@"(\w+): (.+)").Match(line)
        let name = regexMatch.Groups[1].Value
        let outputs = Array.toList (regexMatch.Groups[2].Value.Split(' '))
        (name, outputs)

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Map.ofSeq

let GetNumPaths graphMap baseCaseMemoizationMap startNode = 
    
    let rec buildNumPathsMap memoizationMap currNode = 
        if Map.containsKey currNode memoizationMap then 
            memoizationMap
        else
            let foldSubGraphs accMap nextNode = 
                let subGraph = buildNumPathsMap accMap nextNode
                let toAdd = Map.find nextNode subGraph

                match Map.tryFind currNode subGraph with 
                | None -> Map.add currNode (toAdd) subGraph
                | Some prevValue -> Map.add currNode (toAdd + prevValue) subGraph

            Map.find currNode graphMap
            |> List.fold foldSubGraphs memoizationMap

    buildNumPathsMap baseCaseMemoizationMap startNode
    |> Map.find startNode

let Part1 input = GetNumPaths input (Map.add "out" 1L Map.empty) "you"

let Part2 input = 
    
    let svr = "svr"
    let dac = "dac"
    let fft = "fft"
    let out = "out"

    let defaultBaseCaseMap = Map.add out 0L Map.empty

    let numPathsFunc = GetNumPaths input

    let SVR_to_FFT = numPathsFunc (Map.add fft 1L defaultBaseCaseMap) svr 
    let FFT_to_DAC = numPathsFunc (Map.add dac 1L defaultBaseCaseMap) fft
    let DAC_to_OUT = numPathsFunc (Map.add out 1L defaultBaseCaseMap) dac
    let numA = SVR_to_FFT * FFT_to_DAC * DAC_to_OUT

    let SVR_to_DAC = numPathsFunc (Map.add dac 1L defaultBaseCaseMap) svr 
    let DAC_to_FFT = numPathsFunc (Map.add fft 1L defaultBaseCaseMap) dac 
    let FFT_to_OUT = numPathsFunc (Map.add out 1L defaultBaseCaseMap) fft 
    let numB = SVR_to_DAC * DAC_to_FFT * FFT_to_OUT

    numA + numB