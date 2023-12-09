module Day7

open System.IO
open System.Text.RegularExpressions

// Number indicates it's strength rank
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

// Number indicates it's strength rank
type HandType = 
    | FiveOfAKind = 1
    | FourOfAKind = 2
    | FullHouse = 3
    | ThreeOfAKind = 4
    | TwoPair = 5
    | Pair = 6
    | HighCard = 7

type Hand = {
    Cards: CardType list
    Bid: int
}

let ParseInput filepath = 
    File.ReadLines(filepath)
    |> Seq.map (fun line ->
        let lineMatch = Regex(@"(\w+) (\d+)").Match line
        let handCards = lineMatch.Groups.[1].Value
        let bid = int lineMatch.Groups.[2].Value
        (handCards, bid))
    |> Seq.toList

let GetHandType toProcess = 

    let cardCount = 
        toProcess.Cards
        |> List.fold (fun cardMap nextCard ->
            // Make sure to not count jokers
            match Map.tryFind nextCard cardMap with 
            | Some _ when nextCard = CardType.Joker -> cardMap
            | None -> Map.add nextCard 1 cardMap
            | Some prevCount -> Map.add nextCard (1 + prevCount) cardMap) Map.empty

    let cardCountValues = Map.values cardCount

    let pairCount = 
        cardCountValues
        |> Seq.filter (fun count -> count = 2)
        |> Seq.length

    let tripleCount = 
        cardCountValues
        |> Seq.filter (fun count -> count = 3)
        |> Seq.length

    let fourOfExists = Option.isSome (Seq.tryFind (fun count -> count = 4) cardCountValues)

    let fiveOfExists = Option.isSome (Seq.tryFind (fun count -> count = 5) cardCountValues)

    if fiveOfExists then
        HandType.FiveOfAKind
    elif fourOfExists then
        HandType.FourOfAKind
    elif tripleCount = 1 && pairCount = 1 then
        HandType.FullHouse
    elif tripleCount = 1 then
        HandType.ThreeOfAKind
    elif pairCount = 2 then
        HandType.TwoPair
    elif pairCount = 1 then 
        HandType.Pair
    else
        HandType.HighCard

let CompareHands hand1 hand2 = 
    let hand1Type = GetHandType hand1
    let hand2Type = GetHandType hand2
    match compare hand1Type hand2Type with 
    | 0 -> 
        let rec compareCardPairs (remainingCardPairs: (CardType*CardType) list) = 
            match List.isEmpty remainingCardPairs with 
            | true -> 0
            | false -> 
                let (card1, card2) = List.head remainingCardPairs
                match compare card1 card2 with 
                | 0 -> compareCardPairs (List.tail remainingCardPairs)
                | cardTypeDiff -> cardTypeDiff
        compareCardPairs (List.zip hand1.Cards hand2.Cards)
    | handTypeDiff -> handTypeDiff 

let Part1 (input: (string*int) list) = 
    let cardCharMap = 
        [
            ('A', CardType.Ace)
            ('K', CardType.King)
            ('Q', CardType.Queen)

            ('J', CardType.Jack)

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

    input
    |> List.map (fun (handString, bid) ->
        let cardList = 
            handString
            |> Seq.map (fun handChar -> cardCharMap[handChar])
            |> Seq.toList
        {Hand.Cards = cardList; Bid = bid})
    |> List.sortWith (fun hand1 hand2 -> -1 * (CompareHands hand1 hand2))
    |> List.indexed
    |> List.map (fun (index, hand) -> (index + 1, hand))
    |> List.fold (fun totalWinnings (rank, hand) -> totalWinnings + (rank * hand.Bid)) 0 

let Part2 input = 
    let cardCharMap = 
        [
            ('A', CardType.Ace)
            ('K', CardType.King)
            ('Q', CardType.Queen)

            ('J', CardType.Joker)

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
    0

[<EntryPoint>]
let main _ =
    let input = ParseInput("Input.txt")
    let part1Result = Part1 input
    printfn "Part 1 Result: %d" part1Result // 251216224
    //let part2Result = Part2 input
    //printfn "Part 2 Result: %d" part2Result // 
    0