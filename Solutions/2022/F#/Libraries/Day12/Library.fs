module Day12

open System.IO
open FSharpx.Collections
open System

type QueueEntry = {
    Visited: Set<int*int>
    Pos: int*int
}

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.indexed
    |> Seq.collect (fun (y, line) -> 
        line
        |> Seq.index
        |> Seq.map (fun (x, gridChar) -> ((y,x), gridChar)))
    |> Map.ofSeq

let Part1 input = 
    let startPos = 
        Map.toList input
        |> List.filter (fun (_, gridChar) -> gridChar = 'S')
        |> List.map (fun (gridPos, _) -> gridPos)
        |> List.head

    let endPos = 
        Map.toList input
        |> List.filter (fun (_, gridChar) -> gridChar = 'E')
        |> List.map (fun (gridPos, _) -> gridPos)
        |> List.head
    
    let rec processTraversalQueue gridMap posQueue = 
        if Queue.isEmpty posQueue then 
            failwith "Didn't find top"
        else
            let (nextRecord, remainingQueue) = Queue.uncons posQueue
            let nextPos = nextRecord.Pos
            
            if nextPos = endPos then 
                Set.count nextRecord.Visited
            elif Set.contains nextPos nextRecord.Visited then 
                processTraversalQueue gridMap remainingQueue
            else
                let nextVisited = Set.add nextRecord.Pos nextRecord.Visited
                let checkIfCanMove toCheck = 
                    if not (Map.containsKey toCheck gridMap) then 
                        false
                    else
                        let nextHeight = int gridMap.[nextPos]
                        let toCheckHeight = int gridMap.[toCheck]
                        if abs (toCheckHeight - nextHeight) > 1 then 
                            false
                        else 
                            true
                let traversalFold acc next = 
                    Queue.conj {QueueEntry.Pos = next; Visited = nextVisited} acc


                [
                        fun (y,x) -> (y+1, x);
                        fun (y,x) -> (y-1, x);
                        fun (y,x) -> (y, x+1);
                        fun (y,x) -> (y, x-1);
                ]
                |> List.map (fun traversalFunc -> traversalFunc nextPos)
                |> List.filter checkIfCanMove
                |> List.fold traversalFold remainingQueue
                |> processTraversalQueue gridMap
                    
    let startMap = 
        input
        |> Map.add startPos 'a' 
        |> Map.add endPos '{'

    processTraversalQueue startMap (Queue.conj {QueueEntry.Pos = startPos; Visited = Set.empty} Queue.empty)

let Part2 input = 
    0