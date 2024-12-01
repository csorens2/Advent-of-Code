package Day1

import scala.annotation.tailrec
import scala.io.Source

def parseFile(fileName: String): (List[Int], List[Int]) =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  @tailrec
  def parseLists(remainingLines: List[String], leftList: List[Int], rightList: List[Int]): (List[Int], List[Int]) =
    val lineRegex = "(\\d+)[ ]+(\\d+)".r
    if remainingLines.isEmpty then
      (leftList, rightList)
    else
      val toParse = remainingLines.head
      val nextMatches = lineRegex.findFirstMatchIn(toParse)
      nextMatches match
        case Some(lineMatch) =>
          val leftNum = lineMatch.group(1).toInt
          val rightNum = lineMatch.group(2).toInt
          parseLists(remainingLines.tail, leftNum :: leftList, rightNum :: rightList)
        case None =>
          throw new Exception("Line match not found for: " + toParse)

  val (leftList, rightList) = parseLists(fileSource.getLines().toList, List.empty, List.empty)
  (leftList.reverse, rightList.reverse)

def Part1(input1: List[Int], input2: List[Int]): Int =
  val sortedLeftList = input1.sorted
  val sortedRightList = input2.sorted

  def part1Rec(leftList: List[Int], rightList: List[Int], total: Int): Int =
    if leftList.isEmpty then 
      total
    else
      val leftNum = leftList.head
      val rightNum = rightList.head
      val nextTotal = total + Math.abs(leftNum - rightNum)
      part1Rec(leftList.tail, rightList.tail, nextTotal)

  part1Rec(sortedLeftList, sortedRightList, 0)

def Part2(input1: List[Int], input2: List[Int]): Int =

  @tailrec
  def recPart2(leftList: List[Int], rightList: List[Int], total: Int): Int =
    if leftList.isEmpty then 
      total
    else
      val toFind = leftList.head
      val foundCount = rightList.count(num => num == toFind)
      recPart2(leftList.tail, rightList, total + (toFind * foundCount))

  recPart2(input1, input2, 0)