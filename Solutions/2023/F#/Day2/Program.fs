module Day2

open System.IO
open System.Text.RegularExpressions

type Grab = {
    Balls: (string * int) list
}

type Game = {
    Number: int
    Grabs: Grab list
}

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map (fun line ->
        let gameRegex = Regex(@"Game (\d+):")
        let gameMatch = gameRegex.Match line
        let gameNum = (int gameMatch.Groups.[1].Value)
        let remainingString = line.Substring gameMatch.Value.Length

        let grabRegex = Regex(@".+?(?:[;]|$)")
        let grabMatches = grabRegex.Matches remainingString

        let grabAmounts = 
            grabMatches
            |> Seq.map (fun grabMatch -> 
                let colorCountRegex = Regex(@"(\d+) (\w+)")
                let colorMatches = colorCountRegex.Matches grabMatch.Value
                colorMatches
                |> Seq.map (fun colorsGroups -> 
                    let colorAmount = int colorsGroups.Groups.[1].Value
                    let colorName = colorsGroups.Groups.[2].Value
                    (colorName, colorAmount))
                |> Seq.toList)
            |> Seq.map (fun balls -> {Grab.Balls = balls})
            |> Seq.toList
        {Game.Number = gameNum; Grabs = grabAmounts})
    |> Seq.toList

let BuildGameColorMap game baseMap = 
    game.Grabs
    |> List.fold (fun grabAcc nextGrab -> 
        nextGrab.Balls
        |> List.fold (fun colorAcc (nextColorName, nextColorNum) ->
            match Map.tryFind nextColorName colorAcc with 
            | Some prevMax -> Map.add nextColorName (max prevMax nextColorNum) colorAcc
            | None -> Map.add nextColorName nextColorNum colorAcc
        ) grabAcc
    ) baseMap

let Part1 input = 
    let validGame game = 
        let validColorNumList = [
            ("red", 12)
            ("green", 13)
            ("blue", 14)
        ]
        let gameMap = BuildGameColorMap game Map.empty

        match List.tryFind (fun (colorName, colorNum) -> not (gameMap[colorName] <= colorNum)) validColorNumList with
        | Some _ -> false
        | None -> true

    input
    |> List.filter validGame
    |> List.map (fun game -> game.Number)
    |> List.sum

let Part2 input = 
    input
    |> List.map (fun game -> 
        let powerColorList = [
            ("red")
            ("green")
            ("blue")
        ]
        let baseMap = 
            powerColorList
            |> List.map (fun color -> (color, 0))
            |> Map.ofList
        let gameMap = BuildGameColorMap game baseMap

        List.fold (fun powerAcc nextColor -> powerAcc * gameMap[nextColor]) 1 powerColorList)
    |> List.sum

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 3035
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 66027
    0