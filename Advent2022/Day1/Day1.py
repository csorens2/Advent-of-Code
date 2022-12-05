filePath = r""
file = open(filePath, 'r')
lines = file.readlines()

# Parsing
elfCalories = []

currentCount = 0
for line in lines:
    line = line.rstrip()
    if line == "":
        elfCalories.append(currentCount)
        currentCount = 0
        continue
    currentCount += int(line)

# Part 1 > 67633
maxCalories = max(elfCalories)
print("Max elf calorie count:{}".format(maxCalories))

# Part 2 > 199628
sortedList = sorted(elfCalories)
sortedList.reverse()
topX = 3
sumOfTop = 0
for i in range(0, topX):
    sumOfTop += sortedList[i]
print("Sum of top '{}' elves calories:{}".format(topX, sumOfTop))