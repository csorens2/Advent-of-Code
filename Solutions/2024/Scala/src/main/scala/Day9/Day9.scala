package Day9

import scala.annotation.tailrec
import scala.io.Source
import cats.collections.Heap
import cats.Order

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
        moveBlocks(nextVector, leftIndex + 1, rightIndex)

  val movedVector = moveBlocks(input, 0, input.length - 1)

  var position = 0L
  var checksum = 0L
  for(i <- movedVector.indices)
    checksum =
      movedVector(i) match
        case File(id) => checksum + (position * id)
        case FreeSpace() => checksum
    position = position + 1

  checksum

def Part2(input: Vector[DiskMapSpace]): Int = {

  def generateFreespaceMap(freespaceMap: Map[Int, Heap[Int]]): Map[Int, Heap[Int]]  = {

    val initialMap: Map[Int, Heap[Int]] =
      (1 to 9)
        .foldLeft(Map.empty)((acc, next) => acc + (next -> Heap.empty))

    def processFreespace()

    ???
  }

  ???
}

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