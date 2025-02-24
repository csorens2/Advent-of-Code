module Day10

open System.IO

let ParseInput filepath = 
    let parseLine line = 
        line
        |> Seq.map (fun toConvert -> int toConvert - int '0')
        |> Seq.toArray

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.toArray

let InBounds y x (map: int array array) = 
    if y < 0 || y >= map.Length then 
        false
    else if x < 0 || x >= map.Length then 
        false
    else 
        true

let Part1 input = 
    let inBounds y x = InBounds y x input

    let rec processTrailHead nextY nextX prevNum = 
        if not (inBounds nextY nextX) then 
            Set.empty
        else
            let nextVal = input[nextY][nextX]
            if nextVal - prevNum <> 1 then 
                Set.empty
            else if nextVal = 9 then 
                Set.singleton (nextY, nextX)
            else
                let nextPoints = [
                    (nextY+1, nextX)
                    (nextY-1, nextX)
                    (nextY, nextX+1)
                    (nextY, nextX-1)
                ]
                nextPoints
                |> List.map (fun (y,x) -> processTrailHead y x nextVal)
                |> List.fold (fun acc next -> Set.union acc next ) Set.empty
                
    
    let trailHeads = seq {
        for y in 0 .. input.Length - 1 do 
            for x in 0 .. input[0].Length - 1 do 
                if input[y][x] = 0 then 
                    yield (y,x)
    }
    trailHeads
    |> Seq.map (fun (y,x) -> processTrailHead y x -1)
    |> Seq.map (fun set -> set.Count)
    |> Seq.sum

let Part2 input = 
    let inBounds y x = InBounds y x input

    let rec processTrailHead nextY nextX prevNum = 
        if not (inBounds nextY nextX) then 
            0
        else
            let nextVal = input[nextY][nextX]
            if nextVal - prevNum <> 1 then 
                0
            else if nextVal = 9 then 
                1
            else
                let nextPoints = [
                    (nextY+1, nextX)
                    (nextY-1, nextX)
                    (nextY, nextX+1)
                    (nextY, nextX-1)
                ]
                nextPoints
                |> List.map (fun (y,x) -> processTrailHead y x nextVal)
                |> List.sum
                
    
    let trailHeads = seq {
        for y in 0 .. input.Length - 1 do 
            for x in 0 .. input[0].Length - 1 do 
                if input[y][x] = 0 then 
                    yield (y,x)
    }
    trailHeads
    |> Seq.map (fun (y,x) -> processTrailHead y x -1)
    |> Seq.sum