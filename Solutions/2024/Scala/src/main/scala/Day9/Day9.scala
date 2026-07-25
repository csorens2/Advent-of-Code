package Day9

import scala.annotation.tailrec
import scala.io.Source
import cats.collections.Heap

import DiskMapSpace.File
import DiskMapSpace.FreeSpace
enum DiskMapSpace:
  case File(ID: Int)
  case FreeSpace()

def ParseFile(fileName: String): Vector[DiskMapSpace] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  @tailrec
  def buildDiskMapArray(remainingNums: List[Int], isFile: Boolean, currFileID: Int, finalList: List[DiskMapSpace]): Vector[DiskMapSpace] =
    if remainingNums.isEmpty then
      finalList
        .reverse
        .toVector
    else
      val nextNum = remainingNums.head

      val nextListFolder =
        if isFile then
          (acc: List[DiskMapSpace] , _ : Int) => File(currFileID) :: acc
        else
          (acc: List[DiskMapSpace] , _ : Int) => FreeSpace() :: acc
      val nextList = (0 until nextNum).foldLeft(finalList)(nextListFolder)

      val nextID =
        if isFile then
          currFileID + 1
        else
          currFileID

      buildDiskMapArray(remainingNums.tail, !isFile, nextID, nextList)

  val baseList =
    fileSource
      .getLines()
      .next()
      .map(intChar => intChar - '0')
      .toList

  buildDiskMapArray(baseList, true, 0, List.empty)

def CalculateChecksum(diskMap: Vector[DiskMapSpace]): Long =

  val checksumFolder = (acc: Long, nextIndex: Int) =>
    diskMap(nextIndex) match
      case File(id) => acc + (nextIndex * id)
      case FreeSpace() => acc

  diskMap
    .indices
    .foldLeft(0L)(checksumFolder)

def Part1(input: Vector[DiskMapSpace]): Long =

  @tailrec
  def moveBlocks(currVector: Vector[DiskMapSpace], leftIndex: Int, rightIndex: Int): Vector[DiskMapSpace] =
    if leftIndex == rightIndex then
      currVector
    else
      if currVector(leftIndex).isInstanceOf[File] then
        moveBlocks(currVector, leftIndex + 1, rightIndex)
      else if currVector(rightIndex).isInstanceOf[FreeSpace] then
        moveBlocks(currVector, leftIndex, rightIndex - 1)
      else
        val nextVector =
          currVector
            .updated(leftIndex, currVector(rightIndex))
            .updated(rightIndex, FreeSpace())
        moveBlocks(nextVector, leftIndex + 1, rightIndex - 1)

  val movedVector = moveBlocks(input, 0, input.length - 1)

  CalculateChecksum(movedVector)

def Part2(input: Vector[DiskMapSpace]): Long =

  @tailrec
  def buildFreeSpaceMap(freeSpaceMap: Map[Int, Heap[Int]], currIndex: Int): Map[Int, Heap[Int]] =
    if input.length <= currIndex then
      freeSpaceMap
    else
      if input(currIndex).isInstanceOf[File] then
          buildFreeSpaceMap(freeSpaceMap, currIndex + 1)
      else

        def countFreespace(freeSpaceIndex: Int): Int =
          if freeSpaceIndex == input.length then
            0
          else if input(freeSpaceIndex).isInstanceOf[File] then
            0
          else
            1 + countFreespace(freeSpaceIndex + 1)

        val freeSpaceLength = countFreespace(currIndex)

        val nextMap =
          val heapToUpdate =
            freeSpaceMap.get(freeSpaceLength) match
              case Some(prevHeap) => prevHeap
              case None => Heap.empty
          freeSpaceMap + (freeSpaceLength -> heapToUpdate.add(currIndex))
        buildFreeSpaceMap(nextMap, currIndex + freeSpaceLength)

  @tailrec
  def processFiles(currVector: Vector[DiskMapSpace], currFreeSpaceMap: Map[Int, Heap[Int]], currIndex: Int): Vector[DiskMapSpace] =
    if currIndex <= 0 then
      currVector
    else
      currVector(currIndex) match
        case FreeSpace() => processFiles(currVector, currFreeSpaceMap, currIndex - 1)
        case File(currID) =>
          def countFileSpace(countIndex: Int): Int =
            if countIndex < 0 then
              0
            else
              currVector(countIndex) match
                case FreeSpace() => 0
                case File(countID) if currID != countID => 0
                case File(_) => 1 + countFileSpace(countIndex - 1)

          val fileSpace = countFileSpace(currIndex)

          val possibleFreeSpaceLengths =
            currFreeSpaceMap
              .keys
              .filter(freeSpaceSize => fileSpace <= freeSpaceSize)

          if possibleFreeSpaceLengths.isEmpty then
            processFiles(currVector, currFreeSpaceMap, currIndex - fileSpace)
          else
            val possibleLeftMost =
              possibleFreeSpaceLengths
                .map(length => (currFreeSpaceMap(length).getMin.get, length))
                .filter((pos, _) => pos < currIndex)
                .toList
                .sortBy((pos, _) => pos)
                .headOption
            if possibleLeftMost.isEmpty then
              processFiles(currVector, currFreeSpaceMap, currIndex - fileSpace)
            else
              val (leftMostSpace, leftMostLength) = possibleLeftMost.get

              val foldVector = (acc: Vector[DiskMapSpace], nextStep: Int) =>
                acc
                  .updated(leftMostSpace + nextStep, currVector(currIndex - nextStep))
                  .updated(currIndex - nextStep, currVector(leftMostSpace + nextStep))

              val nextVector = (0 until fileSpace).foldLeft(currVector)(foldVector)

              val nextFreeSpaceMap =
                val heapWithLeftMostSpaceRemoved =
                  val (_, toReturn) = currFreeSpaceMap(leftMostLength).pop.get
                  toReturn

                val mapWithFreeSpaceRemoved =
                  if heapWithLeftMostSpaceRemoved.size == 0 then
                    currFreeSpaceMap.removed(leftMostLength)
                  else
                    currFreeSpaceMap + (leftMostLength -> heapWithLeftMostSpaceRemoved)

                if fileSpace == leftMostLength then
                  mapWithFreeSpaceRemoved
                else
                  val updatedHeap = currFreeSpaceMap(leftMostLength - fileSpace).add(leftMostSpace + fileSpace)
                  mapWithFreeSpaceRemoved + ((leftMostLength - fileSpace) -> updatedHeap)

              processFiles(nextVector, nextFreeSpaceMap, currIndex - fileSpace)

  CalculateChecksum(processFiles(input, buildFreeSpaceMap(Map.empty, 0), input.length - 1))

// Old Part 1
/*
enum DiskMapEntry:
  case File(ID: Int, Length: Int)
  case FreeSpace(Length: Int)

def ParseFile(fileName: String): Vector[DiskMapEntry] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  @tailrec
  def BuildEntryList(remainingNums: List[Int], isFile: Boolean, currFileID: Int, finalEntryList: List[DiskMapEntry]): List[DiskMapEntry] =
    if remainingNums.isEmpty then
      finalEntryList
    else
      val nextNum = remainingNums.head
      val nextEntry =
        if isFile then
          File(currFileID, nextNum)
        else
          FreeSpace(nextNum)
      val nextID =
        if isFile then
          currFileID + 1
        else
          currFileID

      BuildEntryList(remainingNums.tail, !isFile, nextID, nextEntry :: finalEntryList)

  val baseList =
    fileSource
      .getLines()
      .next()
      .map(intChar => intChar - '0')
      .toList

  BuildEntryList(baseList, true, 0, List.empty).reverse.toVector

def Part1(input: Vector[DiskMapEntry]): Long =
  @tailrec
  def processVector(entryVector: Vector[DiskMapEntry], leftIndex: Int, rightIndex: Int, finalFileList: List[File]): List[File] =
    if rightIndex == leftIndex then
      entryVector(leftIndex) match
        case File(id, length) => File(id, length) :: finalFileList
        case FreeSpace(_) => finalFileList
    else
      val nextLeft = entryVector(leftIndex)
      val nextRight = entryVector(rightIndex)
      if !nextLeft.isInstanceOf[FreeSpace] then
        processVector(entryVector, leftIndex + 1, rightIndex, nextLeft.asInstanceOf[File] :: finalFileList)
      else if !nextRight.isInstanceOf[File] then
        processVector(entryVector, leftIndex, rightIndex - 1, finalFileList)
      else
        val leftSpace = nextLeft.asInstanceOf[FreeSpace].Length
        val rightFile = nextRight.asInstanceOf[File]
        if rightFile.Length < leftSpace then
          val nextEntryVector = entryVector.updated(leftIndex, FreeSpace(leftSpace - rightFile.Length))
          processVector(nextEntryVector, leftIndex, rightIndex - 1, rightFile :: finalFileList)
        else if leftSpace < rightFile.Length then
          val nextEntryVector = entryVector.updated(rightIndex, File(rightFile.ID, rightFile.Length - leftSpace))
          val nextFile = File(rightFile.ID, leftSpace)
          processVector(nextEntryVector, leftIndex + 1, rightIndex, nextFile.asInstanceOf[File] :: finalFileList)
        else // Right file fits perfectly
          processVector(entryVector, leftIndex + 1, rightIndex - 1, rightFile :: finalFileList)

  val rawFileList = processVector(input, 0, input.length - 1, List.empty).reverse

  var position = 0L
  var checksum = 0L
  for (file <- rawFileList)
    for (i <- 0 until file.Length)
      checksum = checksum + (position * file.ID)
      position = position + 1

  checksum
*/