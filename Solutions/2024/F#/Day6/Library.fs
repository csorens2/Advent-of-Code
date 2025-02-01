module Day6

open System.IO

type Space = 
    | Open
    | Obstruction
    | Guard

let ParseInput filepath = 
    let parseLine (line: string) = 
        let convertChar char = 
            match char with 
            | '.' -> Open
            | '#' -> Obstruction
            | '^' -> Guard
            | _ -> failwith "Unknown map character"
        line
        |> Seq.toList
        |> List.map convertChar
        |> List.toArray

    File.ReadLines(filepath)
    |> Seq.toList
    |> List.map parseLine
    |> List.toArray

let InBounds y x (map: Space array array) = 
    if y < 0 || x < 0 then 
        false
    else if y >= map.Length || x >= map[0].Length then 
        false
    else
        true

let Part1 (input: Space array array) = 
    let rec travel y x direction (visitedSpaces: Set<int*int>) = 
        let moveFuncs = [
            fun (y,x) -> (y - 1, x)
            fun (y,x) -> (y, x + 1)
            fun (y,x) -> (y + 1, x)
            fun (y,x) -> (y, x - 1)
        ]

        let nextVisitedSpaces = visitedSpaces.Add (y,x)

        let currMoveFunc = moveFuncs.Item direction

        let (nextY, nextX) = currMoveFunc (y,x)

        if not (InBounds nextY nextX input) then 
            nextVisitedSpaces.Count
        else if input[nextY][nextX] = Open || input[nextY][nextX] = Guard then 
            travel nextY nextX direction nextVisitedSpaces
        else 
            travel y x ((direction + 1) % 4) nextVisitedSpaces
    
    let guardList = [
        for y in 0..input.Length - 1 do 
            for x in 0..input[0].Length - 1 do 
                if input[y][x] = Guard then 
                    yield (y,x)
    ]
    
    let (guardY, guardX) = guardList.Head

    travel guardY guardX 0 Set.empty

let Part2 input = 
    0