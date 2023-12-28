module Day10

open System.IO

type Pipe = 
    | Vertical
    | Horizontal
    | NorthEast
    | NorthWest
    | SouthEast
    | SouthWest

type Tile = 
    | PipeTile of Pipe
    | StartTile
    | GroundTile

type Direction = 
    | North
    | South
    | East
    | West

// Remember: The grid starts in 0,0 in the upper left corner  
let ParseInput filepath = 
    let charMap = 
        [
            ('|', PipeTile Vertical)
            ('-', PipeTile Horizontal)
            ('L', PipeTile NorthEast)
            ('J', PipeTile NorthWest)
            ('F', PipeTile SouthEast)
            ('7', PipeTile SouthWest)
            ('S', StartTile)
            ('.', GroundTile)
        ]
        |> Map.ofList

    File.ReadLines(filepath)
    |> Seq.map (fun line ->
        line
        |> Seq.map (fun lineChar -> 
            match Map.tryFind lineChar charMap with 
            | None -> failwith $"Unknown tile char {lineChar}"
            | Some charTile -> charTile)
        |> Seq.toArray)
    |> Seq.toArray

let PipeFits arrivingDirection currPipe =        
    let pipesThatFit = 
        match arrivingDirection with 
        | North -> [Vertical; SouthEast; SouthWest]
        | South -> [Vertical; NorthEast; NorthWest]
        | East -> [Horizontal; NorthWest; SouthWest]
        | West -> [Horizontal; NorthEast; SouthEast]
    pipesThatFit
    |> List.contains currPipe

let GetNextPoints tileType (currY, currX) = 
    let childDirections = 
        match tileType with 
        | StartTile -> [North;South;East;West]
        | PipeTile pipe -> 
            match pipe with 
            | Vertical -> [North; South]
            | Horizontal -> [East; West]
            | NorthEast -> [North; East]
            | NorthWest -> [North; West]
            | SouthEast -> [South; East]
            | SouthWest -> [South; West]
        | GroundTile -> failwith "Attempting to get next points from ground tile"

    childDirections
    |> List.map (fun direction ->
        let nextPos = 
            match direction with 
            | North -> (currY - 1, currX)
            | South -> (currY + 1, currX)
            | East -> (currY, currX + 1)
            | West -> (currY, currX - 1)
        (direction, nextPos))

let GetTile (grid: Tile array array) (targetY, targetX) = 
    if targetY < 0 || grid.Length <= targetY then
        None
    elif targetX < 0 || grid[targetY].Length <= targetX then
        None
    else 
        Some (grid.[targetY].[targetX])

let Part1 (input: Tile array array) = 
    let rec dfsFindLoopLength prevDirection currPos currSteps =
        match GetTile input currPos with
        | None -> None
        | Some GroundTile -> None
        | Some StartTile when currSteps <> 0 -> Some currSteps
        | Some StartTile ->
            GetNextPoints StartTile currPos
            |> List.pick (fun (startingDirection, startingPos) -> 
                let findLoopResult = dfsFindLoopLength startingDirection startingPos 1
                match findLoopResult with 
                | None -> None
                | Some _ -> Some (findLoopResult))
        | Some (PipeTile currPipe) when not (PipeFits prevDirection currPipe) -> None
        | Some (PipeTile currPipe) -> 
            // Need to make sure we don't double-back
            let directionToRemove = 
                match prevDirection with 
                | North -> South
                | South -> North
                | East -> West
                | West -> East
            let (nextDirection, nextPos) = 
                GetNextPoints (PipeTile currPipe) currPos
                |> List.filter (fun (nextDirection, _) -> nextDirection <> directionToRemove)
                |> List.head
            dfsFindLoopLength nextDirection nextPos (currSteps + 1)
             
    let startingY = 
        input
        |> Array.findIndex (fun tileRow -> Array.contains StartTile tileRow)
    let startingX = 
        input[startingY]
        |> Array.findIndex (fun tile -> tile = StartTile)  
    
    match dfsFindLoopLength North (startingY, startingX) 0 with 
    | None -> failwith "Somehow didn't find loop" 
    | Some loopLength -> loopLength / 2

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