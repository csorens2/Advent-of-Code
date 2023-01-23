open System.IO

type SpaceEnum = 
    | Floor = 0 
    | Empty = 1 
    | Occupied = 2

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map (fun line ->
        line |> Seq.map (fun x ->
            match x with
            | '.' -> SpaceEnum.Floor
            | 'L' -> SpaceEnum.Empty
            | '#' -> SpaceEnum.Occupied
            | _ -> failwith "Invalid space"
            ) |> Seq.toList 
        ) |> Seq.toList

let printGrid (grid: SpaceEnum list list) =
    let printRow (subgrid: SpaceEnum list) = 
        let printchar x = 
            match x with
            | SpaceEnum.Floor -> '.'
            | SpaceEnum.Empty -> 'L'
            | SpaceEnum.Occupied -> '#'
        subgrid 
        |> Seq.iter(fun x -> printf "%c " (printchar x))
        printfn""
    grid
    |> Seq.iter(fun x -> printRow x)
    printfn ""

let getVisibleOccupieds (grid:SpaceEnum list list) (y:int) (x:int) = 
    let rec travelDirection (y,x) travelFunc = 
        if x >= 0 && y >= 0 && y < List.length(grid) && x < List.length(grid[0]) then
            0
        else
            match grid[y][x] with
            | SpaceEnum.Floor -> travelDirection (travelFunc (y, x)) travelFunc
            | SpaceEnum.Empty -> 0
            | SpaceEnum.Occupied -> 1
            | _ -> failwith "Unexpected SpaceEnum"
    let directionFuncs = seq {
        (fun (y,x) -> (y-1, x));
        (fun (y,x) -> (y-1, x+1));
        (fun (y,x) -> (y, x+1));
        (fun (y,x) -> (y+1, x+1));
        (fun (y,x) -> (y+1, x));
        (fun (y,x) -> (y+1, x-1));
        (fun (y,x) -> (y, x-1));
        (fun (y,x) -> (y-1, x-1));
    }
    directionFuncs
    |> Seq.fold(fun acc next -> acc + (travelDirection (y,x) next)) 0

let getNearbyOccupieds (grid:SpaceEnum list list) (y:int) (x:int) = 
    let inBoundsCheck grid y x = 
        if (x >= 0 && y >= 0 && y < List.length(grid) && x < List.length(grid[0]) 
                && grid[y][x] = SpaceEnum.Occupied) then
            1
        else
            0
    let adjacentSpots = seq {
        (y-1, x);
        (y-1, x+1);
        (y, x+1);
        (y+1,x+1);
        (y+1,x);
        (y+1,x-1);
        (y, x-1);
        (y-1,x-1);
    }
    adjacentSpots
    |> Seq.fold (fun acc (y2,x2) -> acc + (inBoundsCheck grid y2 x2)) 0


let getOccupiedCount input occupiedFunction minimumForOccupiedFlip= 
    let rec attemptGridUpdate (grid:SpaceEnum list list) surroundingOccupidesFunc = 
        let getNextGridPoint (grid:SpaceEnum list list) y x = 
            let nextOccupiedCount = surroundingOccupidesFunc grid y x
            let prevSeatType = grid[y][x]
            let nextSeatType = 
                match prevSeatType with
                | SpaceEnum.Empty when nextOccupiedCount = 0 -> SpaceEnum.Occupied
                | SpaceEnum.Empty when nextOccupiedCount <> 0 -> SpaceEnum.Empty
                | SpaceEnum.Occupied when nextOccupiedCount >= minimumForOccupiedFlip -> SpaceEnum.Empty
                | SpaceEnum.Occupied when nextOccupiedCount < minimumForOccupiedFlip -> SpaceEnum.Occupied
                | SpaceEnum.Floor -> SpaceEnum.Floor
                | _ -> failwith "Unknown floor type"
            (nextSeatType, prevSeatType = nextSeatType)

        let baseGrid = 
            grid 
            |> List.indexed
            |> List.map (fun (y,row) -> 
                row
                |> List.indexed
                |> List.map(fun (x,_) -> getNextGridPoint grid y x))
        let anyFlipped = 
            baseGrid
            |> List.exists(fun (row) ->
                row
                |> List.exists(fun (_,y) -> not y))
        let prunedGrid = 
                baseGrid
                |> List.map(fun y ->
                    y
                    |> List.map(fun (x,_) -> x))
        if not anyFlipped then
            let countOccupied (row: SpaceEnum list) = 
                row
                |> List.where(fun x -> x = SpaceEnum.Occupied)
                |> List.length
            prunedGrid
            |> Seq.fold (fun acc next -> acc + (countOccupied next)) 0
        else
            printGrid prunedGrid
            attemptGridUpdate prunedGrid occupiedFunction
    
    attemptGridUpdate input occupiedFunction
                    
[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = getOccupiedCount input getNearbyOccupieds 4
    printfn "Part 1 Result: %d" part1Result 
    let part2Result = getOccupiedCount input getVisibleOccupieds 5
    printfn "Part 2 Result: %d" part2Result
    0