import sys
import functools

def ParseInput(fileLines):
    for line in fileLines:
        elfList = line.split(",")
        elfListOfTuples = []
        for elf in elfList:
            timeRange = elf.split("-")
            elfListOfTuples.append((int(timeRange[0]), int(timeRange[1])))
        yield elfListOfTuples

filePath = sys.argv[1]
file = open(filePath, 'r')
fileLines = map(lambda x: x.rstrip(), file.read().splitlines())
parsedLines = list(ParseInput(fileLines))

# Part 1 > 475

def HasFullOverlap(elfGroup):
    elfGroupRangeSet = list(map(lambda elf: set(range(elf[0], elf[1] + 1)), elfGroup))
    groupIntersection = functools.reduce(lambda acc, next: acc.intersection(next), elfGroupRangeSet)
    coveredGroups = len(list(filter(lambda elf: len(elf) == len(groupIntersection), elfGroupRangeSet)))
    return coveredGroups > 0
numFullOverlap = functools.reduce(lambda acc, next: acc + 1 if HasFullOverlap(next) else acc , parsedLines, 0)

print("Number of Elf-Groups with an Elf Fully Overlapped: {}".format(numFullOverlap))

# Part 2 > 825
def HasAnyOverlap(elfGroup):
    elfGroupRangeSet = list(map(lambda elf: set(range(elf[0], elf[1] + 1)), elfGroup))
    groupIntersection = functools.reduce(lambda acc, next: acc.intersection(next), elfGroupRangeSet)
    return len(groupIntersection) > 0
numAnyOverlap = functools.reduce(lambda acc, next: acc + 1 if HasAnyOverlap(next) else acc , parsedLines, 0)

print("Number of Elf-Groups with any amount of overlap: {}".format(numAnyOverlap))