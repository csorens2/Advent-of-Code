package Day2

import scala.annotation.tailrec
import scala.io.Source

enum Trend:
  case Increasing, Decreasing

def parseFile(fileName: String): List[List[Int]] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  def parseLine(toParse: String): List[Int] =
    val lineRegex = "(\\d+)".r
    val matches = lineRegex.findAllMatchIn(toParse)
    matches
      .map(lineMatch => lineMatch.group(1).toInt)
      .toList

  fileSource
    .getLines()
    .map(parseLine)
    .toList

def isSafe(remainingValues: List[Int], prev: Option[Int], numTrend: Option[Trend], usedTolerate: Boolean): Boolean =
  if remainingValues.isEmpty then
    true
  // This one line change solves part 2
  // Very greedy and inefficient, but since there's only at most 10 numbers, not that bad
  else if !usedTolerate && isSafe(remainingValues.tail, prev, numTrend, true) then 
    true
  else if prev.isEmpty then
    isSafe(remainingValues.tail, Some(remainingValues.head), None, usedTolerate)
  else if numTrend.isEmpty then
    if prev.get < remainingValues.head then
      isSafe(remainingValues, prev, Some(Trend.Increasing), usedTolerate)
    else if prev.get > remainingValues.head then
      isSafe(remainingValues, prev, Some(Trend.Decreasing), usedTolerate)
    else
      false
  else
    val nextVal = remainingValues.head
    val diff = Math.abs(prev.get - nextVal)
    if prev.get < nextVal && numTrend.get == Trend.Decreasing then
      false
    else if prev.get > nextVal && numTrend.get == Trend.Increasing then
      false
    else if diff == 0 || diff > 3 then
      false
    else
      isSafe(remainingValues.tail, Some(nextVal), numTrend, usedTolerate)

def Part1(input: List[List[Int]]): Int =
  input
    .map(line => isSafe(line, None, None, true))
    .count(safe => safe == true)

def Part2(input: List[List[Int]]): Int =
  input
    .map(line => isSafe(line, None, None, false))
    .count(safe => safe == true)