module Day4

open System.IO
open System.Text.RegularExpressions

type Card = {
    WinningNumbers: Set<int>
    PlayerNumbers: Set<int>
}

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.fold (fun cardMapAcc line ->
        let cardNumberRegex = Regex(@"(?:Card[ ]+(\d+))")
        let cardNumberMatch = cardNumberRegex.Match line
        let cardNum = (int cardNumberMatch.Groups.[1].Value)
        let remainingLine = line.Substring cardNumberMatch.Value.Length

        let extractNumbers numberString = 
            let numbersRegex = Regex(@"(\d+)")
            numbersRegex.Matches numberString
            |> Seq.map (fun numMatch -> int numMatch.Groups.[1].Value)
            |> Seq.toList

        let numbersRegex = Regex(@".+?(?:[|]|$)")
        let numbersMatches = numbersRegex.Matches remainingLine

        let winningNumbers = Set.ofList (extractNumbers numbersMatches[0].Value)
        let playerNumbers = Set.ofList (extractNumbers numbersMatches[1].Value)
        
        Map.add cardNum {WinningNumbers = winningNumbers; PlayerNumbers = playerNumbers} cardMapAcc) Map.empty

let NumPlayerWinningNumbers card = 
    card.PlayerNumbers
    |> Set.filter (fun nextPlayerNum -> Set.contains nextPlayerNum card.WinningNumbers)
    |> Set.count

let Part1 input = 
    input
    |> Map.map (fun _ card -> 
        let numPlayerWinners = NumPlayerWinningNumbers card
        match numPlayerWinners with
        | 0 -> 0
        | _ -> int (2.0 ** ((float numPlayerWinners) - 1.0)))
    |> Map.fold (fun scoreAcc _ cardScore -> scoreAcc + cardScore) 0

let Part2 input = 
    let rec scratchTickets currCard cardAmounts = 
        match Map.containsKey currCard input with
        | false -> cardAmounts
        | true ->
            let currCardCount = 
                1 + 
                match Map.tryFind currCard cardAmounts with
                | Some cardAmount -> cardAmount
                | None -> 0
            let nextCardAmounts = Map.add currCard currCardCount cardAmounts
            let numPlayerWinners = NumPlayerWinningNumbers input[currCard]
            match numPlayerWinners with
            | 0 -> scratchTickets (currCard + 1) nextCardAmounts
            | _ ->
                [(currCard + 1) .. (currCard + numPlayerWinners)]
                |> List.fold (fun nextCardAmountsAcc childCard -> 
                    match Map.tryFind childCard nextCardAmountsAcc with
                    | Some childCardCount -> Map.add childCard (childCardCount + currCardCount) nextCardAmountsAcc
                    | None -> Map.add childCard currCardCount nextCardAmountsAcc) nextCardAmounts
                |> scratchTickets (currCard + 1)

    scratchTickets 1 Map.empty
    |> Map.fold (fun cardAmountAcc _ cardAmount -> cardAmountAcc + cardAmount) 0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 26914
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 13080971
    0