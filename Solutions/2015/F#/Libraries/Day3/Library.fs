module Day3

open System.IO

type Direction = 
    | North
    | South
    | East
    | West

let ParseInput filepath = 
    let mapChar toMap = 
        match toMap with 
        | '^' -> North
        | 'v' -> South
        | '>' -> East
        | '<' -> West
        | _ -> failwith "Unknown char"

    File.ReadLines(filepath)
    |> Seq.head
    |> Seq.map mapChar
    |> Seq.toList

let DirectionMove direction (startY, startX) = 
    match direction with 
    | North -> (startY - 1, startX)
    | South -> (startY + 1, startX)
    | East -> (startY, startX + 1)
    | West -> (startY, startX - 1)

let Part1 input = 
    let rec processDirections remainingDirections currPos posSet = 
        if List.isEmpty remainingDirections then 
            Set.count posSet
        else
            processDirections 
                remainingDirections.Tail 
                (DirectionMove remainingDirections.Head currPos) 
                (Set.add currPos posSet)
    
    processDirections input (0,0) Set.empty
    

let Part2 input = 
    let rec processDirections remainingDirections humanPos robotPos isRobot posSet =
        if List.isEmpty remainingDirections then 
            Set.count posSet
        else   
            let nextDirection = remainingDirections.Head
            let (nextHumanPos, nextRobotPos) = 
                if isRobot then 
                    (humanPos, DirectionMove nextDirection robotPos) 
                else 
                    (DirectionMove nextDirection humanPos, robotPos) 
            let nextPosSet = 
                posSet
                |> Set.add nextHumanPos
                |> Set.add nextRobotPos
            processDirections remainingDirections.Tail nextHumanPos nextRobotPos (not isRobot) nextPosSet

    processDirections input (0,0) (0,0) false Set.empty