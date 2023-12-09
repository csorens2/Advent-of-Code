module Tests

open Xunit
open Day7

[<Fact>]
let ``GetHandType returns correct HandType`` () = 
    let baseHand = {Hand.Cards = List.empty; Bid = 0}
    // Yes, these technically aren't actual poker hands since only two have 5 cards. 
    // The function should still give us the results we want even with the incomplete hands, since the rest of the cards would be irrelevant.
    // Besides, you should be able to implement this function without taking a dependency on needing 5 cards. 
    let testCases = 
        [
            ({baseHand with Cards = List.empty}, HandType.HighCard)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace]}, HandType.Pair)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.King; CardType.King]}, HandType.TwoPair)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace]}, HandType.ThreeOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace]}, HandType.FourOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace; CardType.Ace]}, HandType.FiveOfAKind)
            ({baseHand with Cards = [CardType.Ace; CardType.Ace; CardType.Ace; CardType.King; CardType.King]}, HandType.FullHouse)
        ]

    for (testHand, expectedHandType) in testCases do
        Assert.Equal(expectedHandType, GetHandType testHand)

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
    Assert.Fail("Not implemented")

[<Fact>]
let ``Part2 Input Test`` () = 
    let input = ParseInput("Input.txt")
    //Assert.Equal(, Part2 input)
    Assert.Fail("Not implemented")