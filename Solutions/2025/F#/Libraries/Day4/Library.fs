module Day4

open System.IO

type Space = 
    | Open
    | Roll

let ParseInput filepath = 
    let parseLine line = 
        line
        |> Seq.map (fun spaceChar -> if spaceChar = '.' then Open else Roll)
        |> Seq.toArray

    File.ReadLines(filepath)
    |> Seq.map parseLine
    |> Seq.toArray

let ProcessGrid grid recursive = 
    let rec processRollSet rollSetToProcess = 
        let shouldBeRemoved coordinateToCheck = 
            let getSurroundingSpaceList (y,x) = 
                [
                    (y-1, x);
                    (y-1, x+1);
                    (y, x+1);
                    (y+1, x+1);
                    (y+1, x);
                    (y+1, x-1);
                    (y, x-1);
                    (y-1, x-1)
                ]

            let surroundingRolls = 
                getSurroundingSpaceList coordinateToCheck
                |> List.filter (fun possibleRoll -> Set.contains possibleRoll rollSetToProcess)

            if (List.length surroundingRolls) < 4 then 
                true
            else 
                false

        let rollsToRemove = 
            rollSetToProcess
            |> Set.filter shouldBeRemoved

        if not recursive then 
            Set.count rollsToRemove
        else if Set.count rollsToRemove = 0 then 
            0
        else 
            let nextRollSet = Set.fold (fun accRollSet toRemove -> Set.remove toRemove accRollSet) rollSetToProcess rollsToRemove

            (Set.count rollsToRemove) + (processRollSet nextRollSet)

    let baseRollSet = 
        seq {
            for y in 0..(Array.length grid - 1) do 
                for x in 0..(Array.length grid[0] - 1) do 
                    if grid[y][x] = Roll then 
                        yield (y,x)
        }
        |> Set.ofSeq

    processRollSet baseRollSet

let Part1 input = 
    ProcessGrid input false

let Part2 input  = 
    ProcessGrid input true

let Part1OLD input = 
    let inGrid (y,x) = 
        if y < 0 || y > Array.length input - 1 then 
            false
        else if x < 0 || x > Array.length input[0] - 1 then 
            false
        else 
            true

    let getSurroundingSpaceList (y,x) = 
        [
            (y-1, x);
            (y-1, x+1);
            (y, x+1);
            (y+1, x+1);
            (y+1, x);
            (y+1, x-1);
            (y, x-1);
            (y-1, x-1)
        ]

    seq {
        for y in 0..(Array.length input - 1) do 
            for x in 0..(Array.length input[0] - 1) do 
                if input[y][x] = Roll then 
                    let numSurroundingPaper = 
                        getSurroundingSpaceList (y,x)
                        |> List.filter (fun coordinates -> inGrid coordinates)
                        |> List.filter (fun (y,x) -> input[y][x] = Roll)
                        |> List.length
                    if numSurroundingPaper < 4 then 
                        yield (y,x)
    }
    |> Seq.length