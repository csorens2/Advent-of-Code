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


let Storage input = 
    let buildMaxBallDict (gameList: Game list) = 
        gameList
        |> List.fold (fun (acc:Map<string,int>) nextGame -> 
            nextGame.Grabs
            |> List.fold (fun innerAcc nextGrab -> 
                nextGrab.Balls
                |> List.fold (fun innerInnerAcc (nextColorName, nextColorNum) ->
                    match Map.tryFind nextColorName innerInnerAcc with 
                    | Some prevMax -> Map.add nextColorName (max prevMax nextColorNum) innerInnerAcc
                    | None -> Map.add nextColorName nextColorNum innerInnerAcc
                ) innerAcc
            ) acc
        ) Map.empty

    let maxDict = buildMaxBallDict input
    ()


let Part1 (input: Game list) = 
    let possibleGames = [
        ("red", 12)
        ("green", 13)
        ("blue", 14)
    ]

    



    0



[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    //let part1Result = Part1 input
    //printfn "Part 1 Result: %d" part1Result // 
    //let part2Result = Part2 input
    //printfn "Part 2 Result: %d" part2Result // 
    0