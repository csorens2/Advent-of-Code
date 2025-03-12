module Day6

open System.IO

type Point = (int*int)

type Space = 
    | Open
    | Obstruction
    | Guard

type Direction = 
    | North
    | South 
    | East
    | West

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
    let spaceArray = 
        File.ReadLines(filepath)
        |> Seq.toList
        |> List.map parseLine
        |> List.toArray

    seq {
        for y in 0..spaceArray.Length - 1 do 
            for x in 0..spaceArray[0].Length - 1 do 
                yield ((y,x), spaceArray[y][x])
    }
    |> Map.ofSeq




// Returns (VisitedSpaces, didItLoop)
let GetVisitedSpaces (spaceMap: Map<Point, Space>) = 
    let rotate currDirection = 
        match currDirection with 
        | North -> East
        | East -> South
        | South -> West
        | West -> North

    let rec travel y x direction (visitedSpaces: Set<int*int*Direction>) =         
        if visitedSpaces.Contains (y,x,direction) then 
            (visitedSpaces, true)
        else
            let nextPoint = 
                match direction with 
                | North -> (y - 1, x)
                | East -> (y, x + 1)
                | South -> (y + 1, x)
                | West -> (y, x - 1)

            let (nextY, nextX) = nextPoint
            let nextVisitedSpaces = visitedSpaces.Add (y,x,direction)

            if not (spaceMap.ContainsKey nextPoint) then 
                (nextVisitedSpaces, false)
            else if spaceMap[nextPoint] = Open || spaceMap[nextPoint] = Guard then 
                travel nextY nextX direction nextVisitedSpaces
            else 
                travel y x (rotate direction) nextVisitedSpaces

    let ((guardY, guardX), _) = 
        spaceMap
        |> Map.toList
        |> List.filter (fun (_,value) -> value = Guard)
        |> List.head
    
    
    let (rawVisitedSpaces, looped) = travel guardY guardX North Set.empty

    let setFoldFunc acc next = 
        let (nextY, nextX, _) = next
        Set.add (nextY, nextX) acc

    let parsedVisitedSpaces = 
        rawVisitedSpaces
        |> Set.fold setFoldFunc Set.empty

    (parsedVisitedSpaces, looped)

let Part1 (input: Map<Point, Space>) = 
    let (visitedSpaces, _) = GetVisitedSpaces input
    visitedSpaces.Count

let Part2 (input: Map<Point, Space>) = 
    let (visitedSpaces, _) = GetVisitedSpaces input

    let mapFunc next = 
        if input[next] = Guard then 
            0
        else 
            let extraBlockMap = 
                input
                |> Map.add next Obstruction

            let (_, looped) = GetVisitedSpaces extraBlockMap
            if looped then 
                1
            else 
                0

    visitedSpaces
    |> Set.toArray
    |> Array.Parallel.map mapFunc
    |> Array.Parallel.sum

    // Original serial version
    // Went from 90 secs to 60
    (*
    let foldFunc acc next = 
        if baseSpaceMap[next] = Guard then 
            acc
        else
            let extraBlockMap = 
                baseSpaceMap
                |> Map.add next Obstruction

            let (_, looped) = GetVisitedSpaces extraBlockMap
            if looped then 
                acc + 1
            else 
                acc

    visitedSpaces
    |> Set.fold foldFunc 0
    *)