module Day7

open System.IO
open System.Text.RegularExpressions

type CardType = 
    | Ace = 1
    | King = 2
    | Queen = 3
    | Jack = 4
    | Ten = 5
    | Nine = 6
    | Eight = 7
    | Seven = 8
    | Six = 9
    | Five = 10
    | Four = 11
    | Three = 12
    | Two = 13
    | Joker = 14

type HandType = 
    | FiveOfAKind = 1
    | FourOfAKind = 2
    | FullHouse = 3
    | ThreeOfAKind = 4
    | TwoPair = 5
    | Pair = 6
    | HighCard = 7

type Hand = {
    Cards: seq<CardType>
    Bid: int
}

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map (fun line ->
        let lineMatch = Regex(@"(\w+) (\d+)").Match line
        let handCards = lineMatch.Groups.[1].Value
        let bid = int lineMatch.Groups.[2].Value
        (handCards, bid))

let GetHandType toProcess = 
    let cardCount = 
        toProcess.Cards
        |> Seq.fold (fun cardMap nextCard ->
            match Map.tryFind nextCard cardMap with 
            | None -> Map.add nextCard 1 cardMap
            | Some prevCount -> Map.add nextCard (1 + prevCount) cardMap) Map.empty

    let nonJokerCardCounts = 
        cardCount
        |> Seq.filter (fun kvp -> not (kvp.Key = CardType.Joker))
        |> Seq.map (fun kvp -> kvp.Value)

    let nonJokerFiveOfExists = Option.isSome (Seq.tryFind (fun count -> count = 5) nonJokerCardCounts)

    let nonJokerFourOfExists = Option.isSome (Seq.tryFind (fun count -> count = 4) nonJokerCardCounts)

    let nonJokerThreeOfExists = Option.isSome (Seq.tryFind (fun count -> count = 3) nonJokerCardCounts)

    let nonJokerNumPairs =
        nonJokerCardCounts
        |> Seq.filter (fun cardCount -> cardCount = 2)
        |> Seq.length

    match Map.tryFind CardType.Joker cardCount with 
    | None -> 
        if nonJokerFiveOfExists then
            HandType.FiveOfAKind
        elif nonJokerFourOfExists then
            HandType.FourOfAKind
        elif nonJokerThreeOfExists && nonJokerNumPairs = 1 then
            HandType.FullHouse
        elif nonJokerThreeOfExists then
            HandType.ThreeOfAKind
        elif nonJokerNumPairs = 2 then
            HandType.TwoPair
        elif nonJokerNumPairs = 1 then
            HandType.Pair
        else 
            HandType.HighCard
    | Some jokerCount -> 
        match jokerCount with 
        | 5 -> HandType.FiveOfAKind
        | 4 -> HandType.FiveOfAKind // All the jokers turn into the one regular card, creating a 5-Of
        | 3 -> 
            if nonJokerNumPairs = 1 then
                HandType.FiveOfAKind
            else
                HandType.FourOfAKind
        | 2 -> 
            if nonJokerThreeOfExists then
                HandType.FiveOfAKind
            elif nonJokerNumPairs = 1 then
                HandType.FourOfAKind
            else
                HandType.ThreeOfAKind
        | 1 -> 
            if nonJokerFourOfExists then
                HandType.FiveOfAKind
            elif nonJokerThreeOfExists then
                HandType.FourOfAKind
            elif nonJokerNumPairs = 2 then
                HandType.FullHouse
            elif nonJokerNumPairs = 1 then
                HandType.ThreeOfAKind
            else
                HandType.Pair
        | _ -> failwith "Invalid number of jokers"

let CompareHands hand1 hand2 = 
    let hand1Type = GetHandType hand1
    let hand2Type = GetHandType hand2
    match compare hand1Type hand2Type with 
    | 0 -> 
        match Seq.tryFind (fun (card1Type, card2Type) -> not (card1Type = card2Type)) (Seq.zip hand1.Cards hand2.Cards) with 
        | None -> 0
        | Some (card1Type, card2Type) -> compare card1Type card2Type
    | handTypeDiff -> handTypeDiff 

let CalculateWinnings rawHands jCharCardType = 
    let cardCharMap = 
        [
            ('A', CardType.Ace)
            ('K', CardType.King)
            ('Q', CardType.Queen)

            ('T', CardType.Ten)
            ('9', CardType.Nine)
            ('8', CardType.Eight)
            ('7', CardType.Seven)
            ('6', CardType.Six)
            ('5', CardType.Five)
            ('4', CardType.Four)
            ('3', CardType.Three)
            ('2', CardType.Two)
        ]
        |> Map.ofList
        |> Map.add 'J' jCharCardType

    rawHands
    |> Seq.map (fun (handString, bid) ->
        let cardSeq = 
            handString
            |> Seq.map (fun handChar -> Map.find handChar cardCharMap)
        {Hand.Cards = cardSeq; Bid = bid})
    |> Seq.sortWith (fun hand1 hand2 -> -1 * (CompareHands hand1 hand2))
    |> Seq.indexed
    |> Seq.map (fun (index, hand) -> (index + 1, hand))
    |> Seq.fold (fun totalWinnings (rank, hand) -> totalWinnings + (rank * hand.Bid)) 0 

let Part1 input = 
    CalculateWinnings input CardType.Jack

let Part2 input = 
    CalculateWinnings input CardType.Joker

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 251216224
    let part2Result = Part2 input
    printfn "Part 2 Result: %d" part2Result // 250825971
    0