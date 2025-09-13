import sys
from enum import Enum

class GameResult(Enum):
    Win = 6
    Draw = 3
    Loss = 0

class Hand(Enum):
    Rock = 1
    Paper = 2
    Scissors = 3

resultDict = { #(Player, Opponent) -> Result
    (Hand.Rock, Hand.Scissors): GameResult.Win,
    (Hand.Rock, Hand.Paper): GameResult.Loss,
    (Hand.Rock, Hand.Rock): GameResult.Draw,
    (Hand.Paper, Hand.Rock): GameResult.Win,
    (Hand.Paper, Hand.Scissors): GameResult.Loss,
    (Hand.Paper, Hand.Paper): GameResult.Draw,
    (Hand.Scissors, Hand.Paper): GameResult.Win,
    (Hand.Scissors, Hand.Rock): GameResult.Loss,
    (Hand.Scissors, Hand.Scissors): GameResult.Draw
}

filePath = sys.argv[1]
file = open(filePath, 'r')
lines = file.read().splitlines()
splitLines = list(map(lambda x: x.split(), iter(lines)))

# Part 1 > 15422
inputToHandDict = {
    'A': Hand.Rock,
    'B': Hand.Paper,
    'C': Hand.Scissors,
    'X': Hand.Rock,
    'Y': Hand.Paper,
    'Z': Hand.Scissors,
}

part1Points = 0
for handPair in splitLines:
    opponentHand = inputToHandDict[handPair[0]]
    playerHand = inputToHandDict[handPair[1]]
    part1Points += playerHand.value + resultDict[(playerHand, opponentHand)].value
print("Part 1 Points: '{}'".format(part1Points))

# Part 2 > 15422
desiredResultDict = {
    'X': GameResult.Loss,
    'Y': GameResult.Draw,
    'Z': GameResult.Win,
}

def GetPlayerHand(desiredResult, opponentHand):
    filterLambda = lambda item: item[1] == desiredResult and item[0][1] == opponentHand
    desiredResult = list(filter(filterLambda, resultDict.items()))[0]
    return desiredResult[0][0]

part2Points = 0
for handPair in splitLines:
    opponentHand = inputToHandDict[handPair[0]]
    desiredResult = desiredResultDict[handPair[1]]
    playerHand = GetPlayerHand(desiredResult, opponentHand)
    part2Points += desiredResult.value + playerHand.value

print("Part 2 Points: '{}'".format(part2Points))