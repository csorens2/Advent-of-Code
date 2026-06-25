package Day5

import scala.LazyList
import scala.collection.immutable
import scala.io.Source

def ParseFile(fileName: String): (List[(Long, Long)], List[Long]) =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)
  val lines = fileSource.getLines().toList

  def parseRange(line: String): (Long, Long) =
    val lineRegex = "(\\d+)-(\\d+)".r
    val lineMatch = lineRegex.findFirstMatchIn(line)
    (lineMatch.get.group(1).toLong, lineMatch.get.group(2).toLong)

  val ranges =
    lines
      .filter(line => line.contains('-'))
      .map(parseRange)

  val ids =
    lines
      .filter(line => "\\d+$".r.matches(line))
      .map(line => line.toLong)

  (ranges, ids)

def Part1(input: (List[(Long, Long)], List[Long])): Long =
  val (ranges, ingredients) = input

  def processIngredient(ingredient: Long): Boolean =
    ranges.exists((left, right) => left <= ingredient && ingredient <= right)

  ingredients
    .count(processIngredient)

def Part2(input: (List[(Long, Long)], List[Long])): Long = {
  def combineRanges(curr: (Long, Long), remaining: List[(Long, Long)]): LazyList[(Long, Long)] =
    if remaining.isEmpty then
      LazyList(curr)
    else
      val (currLeft, currRight) = curr
      val (nextLeft, nextRight) = remaining.head

      // The next range is completely consumed
      if  (currLeft <= nextLeft && nextLeft <= currRight) &&
          (currLeft <= nextRight && nextRight <= currRight) then
        combineRanges(curr, remaining.tail)

      // The next range is partially consumed
      else if (currLeft <= nextLeft && nextLeft <= currRight) &&
              (currRight < nextRight) then
        combineRanges((currLeft, nextRight), remaining.tail)

      // The next range is not consumed at all
      else
        curr #:: combineRanges(remaining.head, remaining.tail)

  val (ranges, _) = input
  val sortedRanges = ranges.sortBy((left, _) => left)

  combineRanges(sortedRanges.head, sortedRanges.tail)
    .toList
    .map((left, right) => right - left + 1)
    .sum
}