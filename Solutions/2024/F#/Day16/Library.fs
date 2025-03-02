module Day16

open System.IO
open FSharpx.Collections

type Tile = 
    | Start
    | End
    | Open
    | Wall

type Direction = 
    | North
    | South
    | East
    | West

let ParseInput filepath = 
    let parseLine (line: string) = 
        let parseChar (nextChar: char) = 
            match nextChar with 
            | 'S' -> Start
            | 'E' -> End
            | '.' -> Open
            | '#' -> Wall
            | _ -> failwith "Unknown char in map"
        line 
        |> Seq.map parseChar
        |> Seq.toArray

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.toArray

let Part1 (input: Tile array array) = 
    let canMove (posY, posX) =
        if posY < 0 || posY >= input.Length then 
            false
        else if posX < 0 || posX >= input[0].Length then
            false
        else if input[posY][posX] = Wall then 
            false
        else 
            true

    let rec traverseMap (posQueue:Queue<(int*int*Direction*int)>) (posMap: Map<(int*int*Direction), int>) (bestPath: int) = 
        if posQueue.IsEmpty then 
            bestPath
        else 
            let ((posY, posX, nextDirection, nextScore), remainingQueue) = posQueue.Uncons
            if input[posY][posX] = End then 
                traverseMap remainingQueue posMap (min nextScore bestPath)
            else 
                let prevScore = 
                    match Map.tryFind (posY, posX, nextDirection) posMap with 
                    | None -> System.Int32.MaxValue
                    | Some (prevVal) -> prevVal
                if prevScore < nextScore then 
                    traverseMap remainingQueue posMap bestPath
                else             
                    let rotateClockwise = 
                        match nextDirection with 
                        | North -> East
                        | East -> South
                        | South -> West
                        | West -> North
                    let rotateCounterClockwise = 
                        match nextDirection with 
                        | North -> West
                        | West -> South
                        | South -> East
                        | East -> North
                    let forward = 
                        match nextDirection with 
                        | North -> (posY - 1, posX)
                        | South -> (posY + 1, posX)
                        | East -> (posY, posX + 1)
                        | West -> (posY, posX - 1)
                    let enqueSeq = seq {
                        yield (posY, posX, rotateClockwise, nextScore + 1000)
                        yield (posY, posX, rotateCounterClockwise, nextScore + 1000)
                        if canMove forward then 
                            let (nextY, nextX) = forward
                            yield (nextY, nextX, nextDirection, nextScore + 1)

                    }
                    let nextQueue = 
                        enqueSeq
                        |> Seq.fold (fun acc next -> Queue.conj next acc) remainingQueue
                    let nextPosMap = Map.add (posY, posX, nextDirection) nextScore posMap
                    traverseMap nextQueue nextPosMap bestPath
  
    let startSearch = seq {
        for y in 0 .. input.Length - 1 do 
            for x in 0 .. input[0].Length - 1 do 
                if input[y][x] = Start then 
                    yield (y,x)
    }
    let (startY, startX) = 
        startSearch
        |> Seq.head

    let startQueue = Queue.conj (startY, startX, East, 0) Queue.empty<(int*int*Direction*int)>
    traverseMap startQueue Map.empty System.Int32.MaxValue
        

let Part2 input = 
    0