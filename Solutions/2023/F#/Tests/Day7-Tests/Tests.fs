module Tests

open Xunit
open Day7

[<Fact>]
let ``GetHandType returns correct HandType`` () = 
    let baseHand = {Hand.Cards = List.empty; Bid = 0}
    let rec testCases_Jacks = 
        ([
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace]}, HandType.FiveOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace; CardType.King]}, HandType.FourOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.King; CardType.King]}, HandType.FullHouse)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.King; CardType.Queen]}, HandType.ThreeOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.King; CardType.King; CardType.Queen]}, HandType.TwoPair)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.King; CardType.Queen; CardType.Jack]}, HandType.Pair)
            ({baseHand with Cards = [CardType.Ace; CardType.King; CardType.Queen; CardType.Jack; CardType.Ten]}, HandType.HighCard)
        ], nameof testCases_Jacks)

    let rec testCases_Jokers_FiveOf = 
        ([
            ({baseHand with Cards = [CardType.Joker; CardType.Joker; CardType.Joker; CardType.Joker; CardType.Joker]}, HandType.FiveOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Joker; CardType.Joker; CardType.Joker; CardType.Joker]}, HandType.FiveOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Joker; CardType.Joker; CardType.Joker]}, HandType.FiveOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.Joker; CardType.Joker]}, HandType.FiveOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace; CardType.Joker]}, HandType.FiveOfAKind)
        ], nameof testCases_Jokers_FiveOf)
    let rec testCases_Jokers_FourOf = 
        ([
            ({baseHand with Cards = [CardType.Ace; CardType.King; CardType.Joker; CardType.Joker; CardType.Joker]}, HandType.FourOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.King; CardType.Joker; CardType.Joker]}, HandType.FourOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.King; CardType.Joker]}, HandType.FourOfAKind)
        ], nameof testCases_Jokers_FourOf)
    let rec testCases_Jokers_ThreeOf = 
        ([
            ({baseHand with Cards = [CardType.Ace; CardType.King; CardType.Queen; CardType.Joker; CardType.Joker]}, HandType.ThreeOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.King; CardType.Queen; CardType.Joker]}, HandType.ThreeOfAKind)
        ], nameof testCases_Jokers_ThreeOf)
    let rec testCases_Jokers_FullHouse = 
        ([
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.King; CardType.King; CardType.Joker]}, HandType.FullHouse)
        ], nameof testCases_Jokers_FullHouse)
    let rec testCases_Jokers_Pair = 
        ([
            ({baseHand with Cards = [CardType.Ace; CardType.King; CardType.Queen; CardType.Ten; CardType.Joker]}, HandType.Pair)
        ], nameof testCases_Jokers_Pair)
    let testCases = [
        testCases_Jacks
        testCases_Jokers_FiveOf
        testCases_Jokers_FourOf
        testCases_Jokers_ThreeOf
        testCases_Jokers_FullHouse
        testCases_Jokers_Pair
    ]

    for (testCase, testName) in testCases do
        for (testHand, expectedHandType) in testCase do
            let actualHandType = GetHandType testHand
            let testSuccess = expectedHandType = actualHandType
            Assert.True(testSuccess, $"Test '{testName}' failed. Expected: '{expectedHandType}' Actual: '{actualHandType}'")

[<Fact>]
let ``CompareHands properly compares different HandTypes`` () = 
    let baseHand = {Hand.Cards = List.empty; Bid = 0}
    let highCardHand = {baseHand with Cards = [CardType.Ace; CardType.King; CardType.Queen; CardType.Jack; CardType.Ten]}
    let fiveOfAKindHand = {baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace]}
    let rec testHands_Negative = ((fiveOfAKindHand, highCardHand), (fun checkNegative -> checkNegative < 0), nameof testHands_Negative)
    let rec testHands_Positive = ((highCardHand, fiveOfAKindHand), (fun checkNegative -> 0 < checkNegative), nameof testHands_Positive)
    
    let testCases = [testHands_Negative; testHands_Positive]
    for ((testHand1, testHand2), expectedValueFunc, testName) in testCases do 
        Assert.True(expectedValueFunc (CompareHands testHand1 testHand2), $"Test '{testName}' failed.")
    
[<Fact>]
let ``CompareHands properly compares using individual card strength when HandType is the same`` () = 
    let baseHand = {Hand.Cards = List.empty; Bid = 0}

    let rec testHands_CompareCard1_Negative = 
        ((
            {baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace]},
            {baseHand with Cards = [CardType.King; CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace]}
        ), (fun checkNegative -> checkNegative < 0),
        nameof testHands_CompareCard1_Negative)

    let rec testHands_CompareCard2_Negative = 
        ((
            {baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace]},
            {baseHand with Cards = [CardType.Ace; CardType.King; CardType.Ace; CardType.Ace; CardType.Ace]}
        ), (fun checkNegative -> checkNegative < 0),
        nameof testHands_CompareCard2_Negative)

    let rec testHands_CompareCard1_Positive = 
        ((
            {baseHand with Cards = [CardType.King; CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace]},
            {baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace]}
        ), (fun checkPositive -> 0 < checkPositive),
        nameof testHands_CompareCard1_Positive)

    let testCases = [testHands_CompareCard1_Negative; testHands_CompareCard2_Negative; testHands_CompareCard1_Positive]
    for ((testHand1, testHand2), expectedValueFunc, testName) in testCases do 
        Assert.True(expectedValueFunc (CompareHands testHand1 testHand2), $"Test '{testName}' failed.")
        

[<Fact>]
let ``Part1 Example Test`` () = 
    let input = ParseInput("ExamplePart1.txt")
    Assert.Equal(6440, Part1 input)

[<Fact>]
let ``Part1 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(251216224, Part1 input)

[<Fact>]
let ``Part2 Example Test`` () = 
    let input = ParseInput("ExamplePart2.txt")
    Assert.Equal(5905, Part2 input)

[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    Assert.Equal(250825971, Part2 input)