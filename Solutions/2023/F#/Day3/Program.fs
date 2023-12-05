module Day3

open System
open System.IO

type EnginePart = {
    PartNumber: int
    Coordinates: (int * int) list
}

type Symbol = {
    Value: char
    Coordinate: (int * int)
}

// Coordinates
// 0,0 is in the upper left, going down and to the right

let ParseInput filepath = 
    let rec recParseLine engineParts symbols enginePart remainingLine currCoordinate = 

        let nextChar = if String.IsNullOrEmpty remainingLine then None else Some remainingLine[0]
            
        let nextEngineParts = 
            match nextChar with
            | None when Option.isSome enginePart -> enginePart.Value :: engineParts
            | None -> engineParts
            | Some nextCharValue when not (System.Char.IsDigit nextCharValue) && Option.isSome enginePart -> enginePart.Value :: engineParts
            | Some _ -> engineParts

        let nextEnginePart = 
            match nextChar with
            | None -> None
            | Some nextCharValue when not (System.Char.IsDigit nextCharValue) -> None
            | Some nextCharValue ->
                let charIntValue = (int nextCharValue) - (int '0')
                match enginePart with
                | None -> 
                    Some {EnginePart.PartNumber = charIntValue; Coordinates = [currCoordinate]} 
                | Some currEnginePart -> 
                    Some {EnginePart.PartNumber = (currEnginePart.PartNumber * 10) + charIntValue; Coordinates = currCoordinate :: currEnginePart.Coordinates}

        let nextSymbols = 
            match nextChar with 
            | None -> symbols
            | Some nextCharValue when System.Char.IsDigit nextCharValue -> symbols
            | Some nextCharValue when nextCharValue = '.' -> symbols
            | Some nextCharValue -> {Symbol.Value = nextCharValue; Coordinate = currCoordinate} :: symbols
                    
        if String.IsNullOrEmpty remainingLine then
            (nextEngineParts, symbols)
        else
            let (currY, currX) = currCoordinate
            let nextCoordinate = (currY, currX + 1)
            let nextRemainingLine = remainingLine.Substring 1  
            recParseLine nextEngineParts nextSymbols nextEnginePart nextRemainingLine nextCoordinate

    File.ReadLines(filepath)
    |> Seq.indexed
    |> Seq.fold (fun (partsListAcc, symbolsListAcc) (yCoord, nextLine) -> 
        recParseLine partsListAcc symbolsListAcc None nextLine (yCoord, 0)) (List.empty, List.empty)

let GetSurroundingCoordinates centerCoordinates = 
    let GetSurroundingCoordinates_SinglePoint (yCoord, xCoord) = 
        seq {
            for nextYCoord in [(yCoord - 1)..(yCoord + 1)] do
                for nextXCoord in [(xCoord - 1)..(xCoord + 1)] do
                    yield (nextYCoord, nextXCoord)
        }
        |> Seq.toList
    centerCoordinates
    |> List.map (fun centerCoordinate -> GetSurroundingCoordinates_SinglePoint centerCoordinate)
    |> List.collect (id)
    |> Set.ofList
    |> Set.filter (fun coordinate -> not (List.contains coordinate centerCoordinates))

let Part1 input = 
    let validateEnginePart part symbols = 
        let partSurroundingCoordinates = GetSurroundingCoordinates part.Coordinates

        let symbolCoordinates = List.map (fun symbol -> symbol.Coordinate) symbols

        symbolCoordinates
        |> List.tryFind (fun symbolCoordinate -> Set.contains symbolCoordinate partSurroundingCoordinates)
        |> Option.isSome
        
    let (partsList, symbolsList) = input
    partsList
    |> List.filter (fun part -> validateEnginePart part symbolsList)
    |> List.map (fun part -> part.PartNumber)
    |> List.sum

let Part2 input = 
    let (partsList, symbolsList) = input

    let gearCoordinates = 
        symbolsList
        |> List.filter (fun symbol -> symbol.Value = '*')
        |> List.map (fun symbol -> symbol.Coordinate)

    let partsWithSurroundingCoords = List.map (fun part -> (part.PartNumber, GetSurroundingCoordinates part.Coordinates)) partsList

    gearCoordinates
    |> List.map (fun gearCoordinate -> 
        let adjacentParts = 
            partsWithSurroundingCoords
            |> List.filter (fun (_, surroundingCoords) -> Set.contains gearCoordinate surroundingCoords)
            |> List.map (fun (partNum, _) -> partNum)
        match List.length adjacentParts with 
        | 2 -> adjacentParts[0] * adjacentParts[1]
        | _ -> 0)
    |> List.sum

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 553079
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 84363105
    0