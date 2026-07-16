package Day4

import scala.annotation.tailrec
import scala.io.Source

def ParseFile(fileName: String): Set[(Int, Int)] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)
  val lines = fileSource.getLines()

  val grid =
    lines
      .map(line => line.toArray)
      .toArray

  val rollPoints =
    for
      y <- grid.indices
      x <- grid(0).indices
      if grid(y)(x) == '@'
    yield (y,x)

  rollPoints.toSet

def NumRollsRemoved(rollPoints: Set[(Int, Int)], recursive: Boolean): Int =
  @tailrec
  def removeRolls(currRolls: Set[(Int, Int)], rollCount: Int): Int =
    def canRemove(pointY: Int, pointX: Int): Boolean =
      val surroundingPoints = List(
        (pointY - 1, pointX),
        (pointY - 1, pointX + 1),
        (pointY, pointX + 1),
        (pointY + 1, pointX + 1),
        (pointY + 1, pointX),
        (pointY + 1, pointX - 1),
        (pointY, pointX - 1),
        (pointY - 1, pointX - 1))

      val numSurroundingRolls =
        surroundingPoints
          .count(point => currRolls.contains(point))

      numSurroundingRolls < 4

    val toRemove = currRolls.filter(canRemove)
    val nextCurrRolls = currRolls.diff(toRemove)

    if !recursive then
      toRemove.size
    else
      val nextRemovedCount = rollCount + toRemove.size
      if toRemove.isEmpty then
        nextRemovedCount
      else
        removeRolls(nextCurrRolls, nextRemovedCount)

  removeRolls(rollPoints, 0)


def Part1(input: Set[(Int, Int)]): Int =
  NumRollsRemoved(input, false)

def Part2(input: Set[(Int, Int)]): Int =
  NumRollsRemoved(input, true)