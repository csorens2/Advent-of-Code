module Day10

open System.IO

type Tile = 
    | VerticalPipe
    | HorizontalPipe
    | NorthEastBend
    | NorthWestBend
    | SouthEastBend
    | SouthWestBend
    | Ground
    | Start

let ParseInput filepath = 
    let charMap = 
        [
            ('|', Tile.VerticalPipe)
            ('-', Tile.HorizontalPipe)
            ('L', Tile.NorthEastBend)
            ('J', Tile.NorthWestBend)
            ('F', Tile.SouthEastBend)
            ('7', Tile.SouthWestBend)
            ('.', Tile.Ground)
            ('S', Tile.Start)
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

// Remember: The grid starts in 0,0 in the upper left

let Part1 (input: Tile array array) = 

    let getNextPoints tileType (currY, currX) prevNode = 
        let nextPointsMap = 
            [
                (Tile.VerticalPipe, [(currY - 1, currX); (currY + 1, currX)])
                (Tile.HorizontalPipe, [(currY, currX - 1); (currY, currX + 1)])
                (Tile.NorthEastBend, [(currY - 1, currX); (currY, currX + 1)])
                (Tile.NorthWestBend, [(currY - 1, currX); (currY, currX - 1)])
                (Tile.SouthEastBend, [(currY + 1, currX); (currY, currX + 1)])
                (Tile.SouthWestBend, [(currY + 1, currX); (currY, currX - 1)])
                (Tile.Start, [(currY + 1, currX);(currY + 1, currX);(currY, currX - 1);(currY, currX - 1)])
            ]   
            |> Map.ofList
        List.filter (fun node -> node <> prevNode) nextPointsMap[tileType]

    let startingY = 
        input
        |> Array.findIndex (fun tileRow -> Array.contains Tile.Start tileRow)
    let startingX = 
        input[startingY]
        |> Array.findIndex (fun tile -> tile = Tile.Start)

    let getTileWithBoundsCheck (pointY, pointX) = 
        if pointY < 0 || input.Length <= pointY then
            None
        elif pointX < 0 || input[pointY].Length <= pointY then
            None
        else 
            Some input.[pointY].[pointX]

    let rec traverseMaze (traversalMap: Map<(int*int), int>) (prevPoint: (int*int)) (currPoint: (int*int)) (currSteps: int) : Map<(int*int), int> option  = 
        // Base cases
        match getTileWithBoundsCheck currPoint with 
        | None -> None
        | Some tile when tile = Tile.Ground -> None
        | Some tile when tile = Tile.Start && currSteps <> 0 -> Some traversalMap
        | Some tile -> 
            match Map.tryFind currPoint traversalMap with 
            | Some _ -> None
            | None ->
                let nextTraversalMap = Map.add currPoint currSteps traversalMap
                let nextSteps = currSteps + 1
                
                let rec checkChildPoints remainingPoints = 
                    match List.isEmpty remainingPoints with 
                    | true -> None
                    | false ->
                        match traverseMaze nextTraversalMap currPoint (List.head remainingPoints) nextSteps with 
                        | None -> checkChildPoints (List.tail remainingPoints)
                        | Some finishedMap -> Some finishedMap

                let nextPoints = getNextPoints tile currPoint prevPoint
                checkChildPoints nextPoints

    let getDistanceToStart (pointX, pointY) = 
        let deltaX = float (pointX - startingX)
        let deltaY = float (pointY - startingY)
        sqrt ((deltaX ** 2) + (deltaY ** 2))

    let traversalMap = 
        (traverseMaze Map.empty (-1,-1) (startingY, startingX) 0).Value

    let traversalSteps = 
        traversalMap
        |> Map.toList


    let (_, test) = 
        traversalMap
        |> Map.toList
        |> List.map (fun (point, stepsToPoint) -> (getDistanceToStart point, stepsToPoint))
        |> List.sortByDescending (fun (distance, _) -> distance)
        |> List.head


    test

let Part2 input = 
    0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    //let part1Result = Part1 input
    //printfn "Part 1 Result: %d" part1Result // 
    //let part2Result = Part2 input
    //printfn "Part 2 Result: %d" part2Result // 
    0