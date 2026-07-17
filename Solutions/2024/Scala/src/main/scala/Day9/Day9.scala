package Day9

import Day9.DiskMapEntry.File
import Day9.DiskMapEntry.FreeSpace

import scala.annotation.tailrec
import scala.io.Source

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
          (currFileID + 1) % 10
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

def Part1(input: Vector[DiskMapEntry]): Int =
  @tailrec
  def processVector(remainingVector: Vector[DiskMapEntry], leftIndex: Int, rightIndex: Int, finalFileList: List[File]): List[File] =
    if rightIndex == leftIndex then
      remainingVector(leftIndex) match
        case File(id,length) => File(id, length) :: finalFileList
        case FreeSpace(_) => finalFileList
    else
      val nextLeft = remainingVector(leftIndex)
      val nextRight = remainingVector(rightIndex)
      if !nextLeft.isInstanceOf[FreeSpace] then
        processVector(remainingVector, leftIndex + 1, rightIndex, nextLeft.asInstanceOf[File] :: finalFileList)
      else if !nextRight.isInstanceOf[File] then
        processVector(remainingVector, leftIndex, rightIndex - 1, finalFileList)
      else
        val leftSpace = nextLeft.asInstanceOf[FreeSpace].Length
        val rightFile = nextRight.asInstanceOf[File]
        if rightFile.Length < leftSpace then
          val nextRemainingVector = remainingVector.updated(leftIndex, FreeSpace(leftSpace - rightFile.Length))
          processVector(nextRemainingVector, leftIndex, rightIndex - 1, rightFile :: finalFileList)
        else if leftSpace < rightFile.Length then
          val nextRemainingVector = remainingVector.updated(rightIndex, File(rightFile.ID, rightFile.Length - leftSpace))
          val nextFile = File(rightFile.ID, leftSpace)
          processVector(nextRemainingVector, leftIndex + 1, rightIndex, nextFile.asInstanceOf[File] :: finalFileList)
        else // Right file fits perfectly
          processVector(remainingVector, leftIndex + 1, rightIndex - 1, rightFile :: finalFileList)

  val rawFileList = processVector(input, 0, input.length - 1, List.empty).reverse

  var position = 0
  var checksum = 0
  for(file <- rawFileList)
    for (i <- 0 until file.Length)
      checksum = checksum + (position * file.ID)
      position = position + 1

  checksum


def Part2(input: Vector[DiskMapEntry]): Int =
  ???