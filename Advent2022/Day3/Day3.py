import sys
import functools
from string import ascii_lowercase
from string import ascii_uppercase

filePath = sys.argv[1]
file = open(filePath, 'r')
lines = file.read().splitlines()

priorityDict = { }
index = 1
asciiChars = ascii_lowercase + ascii_uppercase
for char in asciiChars:
    priorityDict[char] = index
    index += 1

# Part 1 > 7990
rucksackSums = 0
for line in lines:
    leftHalf = line[:len(line)//2]
    rightHalf = line[len(line)//2:]
    intersectingChars = set(leftHalf).intersection(set(rightHalf))
    intersectingPriorities = map(lambda item: priorityDict[item], intersectingChars)
    rucksackSums += sum(intersectingPriorities)
print("Total Rucksack Priority: '{}'".format(rucksackSums))

# Part 2 > 2602
def NextThreeLinesIter(lineList):
    nextSet = []
    for line in lineList:
        if len(nextSet) == 3:
            yield nextSet
            nextSet = []
        nextSet.append(line)
    yield nextSet

groupSums = 0
for lineTriple in NextThreeLinesIter(lines):
    lineTripleSets = map(lambda x: set(x), lineTriple)
    intersectingCharSet = functools.reduce(lambda acc, x: acc.intersection(set(x)), lineTripleSets)
    intersectingCharValue = priorityDict[list(intersectingCharSet)[0]]
    groupSums += intersectingCharValue
print("Elf Group Sums: '{}'".format(groupSums))