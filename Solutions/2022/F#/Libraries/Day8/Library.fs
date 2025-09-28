module Day8

open System.IO

let ParseInput filepath = 
    let lineMapFunc line = 
        line
        |> Seq.map (fun nextChar -> int nextChar - int '0')
        |> Seq.toArray
        
    File.ReadLines(filepath)
    |> Seq.map lineMapFunc
    |> Seq.toArray

let InsideGrid grid (y,x) = 
    if y < 0 || y >= Array.length grid then 
        false
    elif x < 0 || x >= Array.length grid.[0] then 
        false
    else
        true

let Part1 (input: int array array) = 
    let processTree (y,x) = 
        let rec toEdge baseHeight nextTreeFunc (y,x) = 
            if not (InsideGrid input (y,x)) then 
                true
            else 
                let nextTree = input[y][x]
                if nextTree < baseHeight then 
                    toEdge baseHeight nextTreeFunc (nextTreeFunc (y,x))
                else
                    false
        let traversalFunction = [
            fun (y,x) -> (y+1, x);
            fun (y,x) -> (y-1, x);
            fun (y,x) -> (y, x+1);
            fun (y,x) -> (y, x-1);
        ]
        traversalFunction
        |> List.map (fun travFunc -> toEdge (input.[y].[x]) travFunc (travFunc (y,x)))
        |> List.fold (fun acc next -> acc || next) false
    
    let toProcess = seq {
        for y = 0 to (Array.length input) - 1 do 
            for x = 0 to (Array.length input.[0]) - 1 do  
                yield (y,x)
    }

    toProcess
    |> Seq.map processTree
    |> Seq.map (fun seenFromEdge -> if seenFromEdge then 1 else 0)
    |> Seq.sum

let Part2 input = 
    let processTree (y,x) = 
        let rec countTrees baseHeight nextTreeFunc (y,x) =
            if not (InsideGrid input (y,x)) then 
                0
            else
                let nextTree = input[y][x]
                if nextTree < baseHeight then 
                    1 + (countTrees baseHeight nextTreeFunc (nextTreeFunc (y,x)))
                else
                    1
                    
        let traversalFunction = [
            fun (y,x) -> (y+1, x);
            fun (y,x) -> (y-1, x);
            fun (y,x) -> (y, x+1);
            fun (y,x) -> (y, x-1);
        ]
        traversalFunction
        |> List.map (fun travFunc -> countTrees (input.[y].[x]) travFunc (travFunc (y,x)))
        |> List.fold (fun acc next -> acc * next) 1

    let toProcess = seq {
        for y = 0 to (Array.length input) - 1 do 
            for x = 0 to (Array.length input.[0]) - 1 do  
                yield (y,x)
    }

    toProcess
    |> Seq.map processTree
    |> Seq.max