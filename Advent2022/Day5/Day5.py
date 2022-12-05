import re

filePath = r""
file = open(filePath, 'r')
lines = file.readlines()

# Parsing
initialHeight = 0
stackCount = 0
for line in lines:
    if '1' in line:
        stackCount = int(re.findall("[0-9]+", line).pop())
        break
    initialHeight+=1

crateStacks = [[] for _ in range(stackCount+1)]
for i in range(initialHeight-1, -1, -1):
    line = lines[i]
    crateIndex = 0
    for i in range(1, len(line), 4):
        possibleCrate = line[i]
        if line[i-1] == '[':
            crateStacks[int((i+3)/4)].append(possibleCrate)

# Part 1 > VCTFTJQCG
'''
for line in lines:
    if line.find("move") == -1:
        continue
    match = re.search("move (\d*) from (\d*) to (\d*)", line)
    amount = int(match.group(1))
    source = int(match.group(2))
    target = int(match.group(3))
    for _ in range(0,amount):
        crateStacks[target].append(crateStacks[source].pop())
'''

# Part 2 > GCFGLDNJZ
'''
for line in lines:
    if line.find("move") == -1:
        continue
    match = re.search("move (\d*) from (\d*) to (\d*)", line)
    amount = int(match.group(1))
    source = int(match.group(2))
    target = int(match.group(3))
    startIndex = len(crateStacks[source]) - amount
    for i in range (startIndex, len(crateStacks[source])):
        nextCrate = crateStacks[source][i]
        crateStacks[target].append(nextCrate)
    for i in range (startIndex, len(crateStacks[source])):
        crateStacks[source].pop()
'''