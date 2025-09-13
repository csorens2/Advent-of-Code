import sys

filePath = sys.argv[1]
file = open(filePath, 'r')
inputList = list(file.read().splitlines()[0])

def MarkerStartFinder(bufferList, markerSequenceLength):
    for i in range(0, len(bufferList)):
        sequenceSet = set()
        for j in range(i, i + markerSequenceLength):
            nextItem = bufferList[j]
            if nextItem in sequenceSet:
                break
            sequenceSet.add(nextItem)
        if len(sequenceSet) == markerSequenceLength:
            return i + markerSequenceLength

# Part 1 > 1855
startOfPacketMarker = MarkerStartFinder(inputList, 4)
print("Start of Packet Marker: '{}'".format(startOfPacketMarker))

# Part 2 > 3256
startOfMessageMarker = MarkerStartFinder(inputList, 14)
print("Start of Message Marker: '{}'".format(startOfMessageMarker))