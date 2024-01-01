module Day11

open System.IO

type Space = 
    | Empty
    | Galaxy

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map (fun line ->
        line
        |> Seq.map (fun nextChar -> 
            match nextChar with 
            | '.' -> Empty
            | '#' -> Galaxy
            | _ -> failwith $"Failed attempting to parse unknown character: '{nextChar}'")
        |> Seq.toArray)
    |> Seq.toArray

let UniverseDistancesSum universe expansionMultiplier = 

    let rowCount = Array.length universe
    let colCount = Array.length universe.[0]

    let rowList = [0..rowCount - 1]
    let colList = [0..colCount - 1]

    let galaxies = 
        rowList
        |> List.fold (fun galaxyPoints row -> 
            colList
            |> List.fold (fun subGalaxyPoints col -> 
                match universe.[row].[col] with 
                | Empty -> subGalaxyPoints
                | Galaxy -> (row, col) :: subGalaxyPoints
            ) galaxyPoints
        ) List.empty

    let (emptyRows, emptyCols) = 
        rowList
        |> List.fold (fun (emptyRowAcc, emptyColAcc) yPos -> 
            colList
            |> List.fold (fun (subEmptyRowAcc, subEmptyColAcc) xPos -> 
                match universe.[yPos].[xPos] with 
                | Empty -> (subEmptyRowAcc, subEmptyColAcc)
                | Galaxy -> (Set.remove yPos subEmptyRowAcc, Set.remove xPos subEmptyColAcc)
            ) (emptyRowAcc, emptyColAcc)
        ) (Set.ofList rowList, Set.ofList colList)

    let rec getDistanceSum remainingGalaxies currSum =
        match remainingGalaxies with 
        | [] -> currSum
        | [_] -> currSum
        | nextGalaxy :: galaxiesToCompare -> 
            let (nextGalaxyY, nextGalaxyX) = nextGalaxy
            let nextSum = 
                galaxiesToCompare
                |> List.map (fun (toCompareY, toCompareX) -> 
                    let startY = min nextGalaxyY toCompareY
                    let endY = max nextGalaxyY toCompareY
                    let startX = min nextGalaxyX toCompareX
                    let endX = max nextGalaxyX toCompareX
                    let rowJumps = Set.count (Set.filter (fun emptyRow -> startY <= emptyRow && emptyRow <= endY) emptyRows)
                    let colJumps = Set.count (Set.filter (fun emptyCol -> startX <= emptyCol && emptyCol <= endX) emptyCols)

                    let yDistance = uint64 ((endY - startY) - rowJumps + (rowJumps * expansionMultiplier))
                    let xDistance = uint64 ((endX - startX) - colJumps + (colJumps * expansionMultiplier))
                    yDistance + xDistance)
                |> List.sum
            getDistanceSum galaxiesToCompare (currSum + nextSum)
                
    getDistanceSum galaxies 0UL

let Part1 input = UniverseDistancesSum input 2

let Part2 input = UniverseDistancesSum input 1000000

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 9565386
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 857986849428
    0