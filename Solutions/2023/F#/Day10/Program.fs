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

/// The path will be in reverse order, with the start at the end.
let GetLoopPath tileGrid = 
    let rec getLoopPath prevDirection currPos loopPath =
        match GetTile tileGrid currPos with 
        | None -> None
        | Some GroundTile -> None
        | Some StartTile when not (List.isEmpty loopPath) -> Some loopPath
        | Some StartTile ->
            GetNextPoints StartTile currPos 
            |> List.pick (fun (startingDirection, startingPos) ->
                match getLoopPath startingDirection startingPos [currPos] with 
                | None -> None
                | Some loopPathResult -> Some (Some (loopPathResult)))
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
            getLoopPath nextDirection nextPos (currPos :: loopPath)

    let startingY = 
        tileGrid
        |> Array.findIndex (fun tileRow -> Array.contains StartTile tileRow)
    let startingX = 
        tileGrid[startingY]
        |> Array.findIndex (fun tile -> tile = StartTile) 

    match getLoopPath North (startingY, startingX) List.empty with
    | None -> failwith "Somehow didn't find loop"
    | Some loopPath -> loopPath

let Part1 input = 
    (List.length (GetLoopPath input)) / 2
    
let Part2 (input: Tile array array) = 

    (*
    let pipeIsVertex pipe = List.contains pipe [NorthEast; NorthWest; SouthEast; SouthWest]

    // We can't assume that the start is the first vertex, since the loop may go straight through the start
    // We could build the area starting from the first vertex we find, but that complicates the code a lot
    // Since I like simplicity, I'm gonna opt for finding the path of the loop, taking the first and last positions,
    // and using those to find if the start is a vertex
    let loopPath = GetLoopPath input

    let firstVertex = List.find (fun pos -> 
            match GetTile input pos with 
            | Some (PipeTile pipe) when pipeIsVertex pipe -> true
            | _ -> false) loopPath

    let remainingPath = 
        loopPath
        |> List.skipWhile (fun )
    *)

    0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 
    //let part2Result = Part2 input
    //printfn "Part 2 Result: %d" part2Result // 
    0