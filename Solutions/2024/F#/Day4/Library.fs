module Day4

open System.IO

type Point(y: int, x: int) =
    member this.y = y
    member this.x = x
    member this.Value (map: char array array) = 
        map[y][x]
    member this.InBounds (map: char array array) = 
        if y < 0 || x < 0 then 
            false
        else if y >= map.Length || x >= map[0].Length then 
            false
        else 
            true

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map(fun line -> line.ToCharArray())
    |> Seq.toArray

let Part1 input = 
    let rec processXmas (currPoint: Point) chainLength movementFunc = 
        if not (currPoint.InBounds(input)) then 
            false
        else
            match chainLength with 
            | 0 when currPoint.Value(input) = 'X' -> processXmas (movementFunc currPoint) (chainLength + 1) movementFunc
            | 1 when currPoint.Value(input) = 'M' -> processXmas (movementFunc currPoint) (chainLength + 1) movementFunc
            | 2 when currPoint.Value(input) = 'A' -> processXmas (movementFunc currPoint) (chainLength + 1) movementFunc
            | 3 when currPoint.Value(input) = 'S' -> true
            | _ -> false

    let processXPoint (xPoint: Point) =
        let movementFuncs = [
            fun (point: Point) -> Point(point.y - 1, point.x)
            fun (point: Point) -> Point(point.y - 1, point.x + 1)
            fun (point: Point) -> Point(point.y, point.x + 1)
            fun (point: Point) -> Point(point.y + 1, point.x + 1)
            fun (point: Point) -> Point(point.y + 1, point.x)
            fun (point: Point) -> Point(point.y + 1, point.x - 1)
            fun (point: Point) -> Point(point.y, point.x - 1)
            fun (point: Point) -> Point(point.y - 1, point.x - 1)
        ]
        movementFuncs
        |> List.map(fun moveFunc -> processXmas xPoint 0 moveFunc)
        |> List.filter id
        |> List.length

    seq {
        for y in 0..input.Length - 1 do 
            for x in 0..input[0].Length - 1 do 
                if Point(y,x).Value(input) = 'X' then 
                    yield processXPoint (Point(y,x))

    }
    |> Seq.sum

let Part2 (input: char array array) = 
    let processAPoint (currPoint: Point) = 
        let pointY = currPoint.y
        let pointX = currPoint.x

        let pointList = [
            // M on top
            [
                (Point(pointY - 1, pointX - 1), 'M');
                (Point(pointY - 1, pointX + 1), 'M');
                (Point(pointY + 1, pointX - 1), 'S');
                (Point(pointY + 1, pointX + 1), 'S')
            ];
            // M on the right
            [
                (Point(pointY - 1, pointX - 1), 'S');
                (Point(pointY - 1, pointX + 1), 'M');
                (Point(pointY + 1, pointX - 1), 'S');
                (Point(pointY + 1, pointX + 1), 'M')
            ];
            // M on the bottom
            [
                (Point(pointY - 1, pointX - 1), 'S');
                (Point(pointY - 1, pointX + 1), 'S');
                (Point(pointY + 1, pointX - 1), 'M');
                (Point(pointY + 1, pointX + 1), 'M')
            ];
            // M on the left
            [
                (Point(pointY - 1, pointX - 1), 'M');
                (Point(pointY - 1, pointX + 1), 'S');
                (Point(pointY + 1, pointX - 1), 'M');
                (Point(pointY + 1, pointX + 1), 'S')
            ]
        ]

        let forAllFunc ((pointToCheck, charToCheck): Point*char) = 
            pointToCheck.InBounds(input) && pointToCheck.Value(input) = charToCheck
            
        if List.exists (fun pointCharList -> List.forall forAllFunc pointCharList) pointList then 
            1
        else 
            0
                
    seq {
        for y in 0..input.Length - 1 do 
            for x in 0..input[0].Length - 1 do 
                if Point(y,x).Value(input) = 'A' then 
                    yield processAPoint (Point(y,x))

    }
    |> Seq.sum