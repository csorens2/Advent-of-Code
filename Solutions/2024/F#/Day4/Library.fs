module Day4

open System.IO

type Point = (int*int)

let ParseInput filepath = 
    let linesArray = 
        File.ReadLines(filepath)
        |> Seq.map(fun line -> line.ToCharArray())
        |> Seq.toArray

    seq {
        for y in 0..linesArray.Length - 1 do 
            for x in 0..linesArray[0].Length - 1 do 
                yield ((y,x), linesArray[y][x])
    }
    |> Map.ofSeq

let Part1 (input: Map<Point,char>) = 
    let rec processXmas currPoint chainLength movementFunc = 
        if not (input.ContainsKey currPoint) then 
            false
        else
            let currPointValue = input[currPoint]
            match chainLength with 
            | 0 when currPointValue = 'X' -> processXmas (movementFunc currPoint) (chainLength + 1) movementFunc
            | 1 when currPointValue = 'M' -> processXmas (movementFunc currPoint) (chainLength + 1) movementFunc
            | 2 when currPointValue = 'A' -> processXmas (movementFunc currPoint) (chainLength + 1) movementFunc
            | 3 when currPointValue = 'S' -> true
            | _ -> false

    let processXPoint xPoint =
        let movementFuncs = [
            fun ((y,x)) -> (y - 1, x)
            fun ((y,x)) -> (y - 1, x + 1)
            fun ((y,x)) -> (y, x + 1)
            fun ((y,x)) -> (y + 1, x + 1)
            fun ((y,x)) -> (y + 1, x)
            fun ((y,x)) -> (y + 1, x - 1)
            fun ((y,x)) -> (y, x - 1)
            fun ((y,x)) -> (y - 1, x - 1)
        ]

        movementFuncs
        |> List.map(fun moveFunc -> processXmas xPoint 0 moveFunc)
        |> List.filter id
        |> List.length

    seq {
        for (point, charVal) in Map.toSeq input do 
            if charVal = 'X' then 
                yield processXPoint (point)
    }
    |> Seq.sum

let Part2 (input: Map<Point,char>) = 
    let processAPoint currPoint = 
        let (pointY, pointX) = currPoint

        let pointList = [
            // M on top
            [
                ((pointY - 1, pointX - 1), 'M');
                ((pointY - 1, pointX + 1), 'M');
                ((pointY + 1, pointX - 1), 'S');
                ((pointY + 1, pointX + 1), 'S')
            ];
            // M on the right
            [
                ((pointY - 1, pointX - 1), 'S');
                ((pointY - 1, pointX + 1), 'M');
                ((pointY + 1, pointX - 1), 'S');
                ((pointY + 1, pointX + 1), 'M')
            ];
            // M on the bottom
            [
                ((pointY - 1, pointX - 1), 'S');
                ((pointY - 1, pointX + 1), 'S');
                ((pointY + 1, pointX - 1), 'M');
                ((pointY + 1, pointX + 1), 'M')
            ];
            // M on the left
            [
                ((pointY - 1, pointX - 1), 'M');
                ((pointY - 1, pointX + 1), 'S');
                ((pointY + 1, pointX - 1), 'M');
                ((pointY + 1, pointX + 1), 'S')
            ]
        ]

        let forAllFunc (pointToCheck, charToCheck) = 
            match input.TryFind pointToCheck with 
            | Some(inputVal) when inputVal = charToCheck -> true
            | Some(_) -> false
            | None -> false
            
        if List.exists (fun pointCharList -> List.forall forAllFunc pointCharList) pointList then 
            1
        else 
            0
       
    seq {
        for (point, charVal) in Map.toSeq input do 
            if charVal = 'A' then 
                yield processAPoint (point)
    }
    |> Seq.sum