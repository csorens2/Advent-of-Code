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
    | Start
    | Ground

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
            ('S', Start)
            ('.', Ground)
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
    match arrivingDirection with 
        | North -> [Vertical; SouthEast; SouthWest]
        | South -> [Vertical; NorthEast; NorthWest]
        | East -> [Horizontal; NorthWest; SouthWest]
        | West -> [Horizontal; NorthEast; SouthEast]
    |> List.contains currPipe

let GetNextPoints tileType (currY, currX) = 
    match tileType with 
        | Start -> [North;South;East;West]
        | PipeTile pipe -> 
            match pipe with 
                | Vertical -> [North; South]
                | Horizontal -> [East; West]
                | NorthEast -> [North; East]
                | NorthWest -> [North; West]
                | SouthEast -> [South; East]
                | SouthWest -> [South; West]
        | Ground -> failwith "Attempting to get next points from ground tile"
    |> List.map (fun direction ->
        let nextPos = 
            match direction with 
            | North -> (currY - 1, currX)
            | South -> (currY + 1, currX)
            | East -> (currY, currX + 1)
            | West -> (currY, currX - 1)
        (direction, nextPos))

let InsideGrid (grid: Tile array array) (targetY, targetX) = 
    if targetY < 0 || grid.Length <= targetY then
        false
    elif targetX < 0 || grid[targetY].Length <= targetX then
        false
    else 
        true

/// Will return a full loop of (Start :: Rest of Loop :: Start)
let GetLoopPath tileGrid = 
    let rec getLoopPath prevDirection currPos loopPath =
        let nextLoopPath = currPos :: loopPath
        match InsideGrid tileGrid currPos with 
        | false -> None 
        | true -> 
            let (currY, currX) = currPos
            match tileGrid.[currY].[currX] with 
            | Ground -> None
            | Start when not (List.isEmpty loopPath) -> Some loopPath
            | Start ->
                GetNextPoints Start currPos 
                |> List.pick (fun (startingDirection, startingPos) ->
                    match getLoopPath startingDirection startingPos nextLoopPath with 
                    | None -> None
                    | Some loopPathResult -> Some (Some (loopPathResult)))
            | PipeTile currPipe when not (PipeFits prevDirection currPipe) -> None
            | PipeTile currPipe -> 
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
                getLoopPath nextDirection nextPos nextLoopPath

    let startingY = 
        tileGrid
        |> Array.findIndex (fun tileRow -> Array.contains Start tileRow)
    let startingX = 
        tileGrid[startingY]
        |> Array.findIndex (fun tile -> tile = Start) 

    match getLoopPath North (startingY, startingX) List.empty with
    | None -> failwith "Somehow didn't find loop"
    | Some loopPath -> (startingY, startingX) :: loopPath

let Part1 input = (List.length (GetLoopPath input)) / 2

let Part2 (input: Tile array array) =

    let loopPath = GetLoopPath input

    let loopSet = Set.ofList loopPath

    let (minY, _) = List.minBy (fun (yCoord, _) -> yCoord) loopPath
    let (maxY, _) = List.maxBy (fun (yCoord, _) -> yCoord) loopPath
    let (_, minX) = List.minBy (fun (_, xCoord) -> xCoord) loopPath
    let (_, maxX) = List.maxBy (fun (_, xCoord) -> xCoord) loopPath

    let startReplacedGrid = 
        let (startY, startX) = List.head loopPath
        let firstPipe = List.item 1 loopPath
        let lastPipe = List.findBack (fun pos -> pos <> (startY, startX)) loopPath

        let startPipeList = 
            [
                ([(startY - 1, startX); (startY + 1, startX)], Vertical)
                ([(startY, startX - 1); (startY, startX + 1)], Horizontal)
                ([(startY - 1, startX); (startY, startX + 1)], NorthEast)
                ([(startY - 1, startX); (startY, startX - 1)], NorthWest)
                ([(startY + 1, startX); (startY, startX + 1)], SouthEast)
                ([(startY + 1, startX); (startY, startX - 1)], SouthWest)
            ]

        let (_, newStartPipe) = 
            startPipeList
            |> List.find (fun (pipeList, _) -> (List.sort pipeList) = (List.sort [firstPipe; lastPipe]))

        input
        |> Array.map (fun rowTiles ->
            rowTiles
            |> Array.map (fun tile ->
                match tile = Start with 
                | true -> (PipeTile newStartPipe)
                | false -> tile))

    [minY..maxY]
    |> List.fold (fun gridCount yCoord -> 
        let (_, rowCount) = 
            [minX..maxX]
            |> List.fold (fun (insideLoop, subGroundCount) xCoord -> 
                if Set.contains (yCoord, xCoord) loopSet then
                    match startReplacedGrid.[yCoord].[xCoord] with 
                    | PipeTile pipe when List.contains pipe [Vertical; SouthEast; SouthWest] -> (not insideLoop, subGroundCount)
                    | _ -> (insideLoop, subGroundCount)
                elif insideLoop then
                    (insideLoop, subGroundCount + 1)
                else
                    (insideLoop, subGroundCount)
            ) (false, 0) 

        let test = 1

        gridCount + rowCount
    ) 0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 
    0