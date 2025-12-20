package Day8

import scala.io.Source
import math.{sqrt, pow}

case class Point(X: Int, Y: Int, Z: Int)

def GetDistance(p1: Point, p2: Point): Double =
  val dx = p2.X - p1.X
  val dy = p2.Y - p1.Y
  val dz = p2.Z - p1.Z
  math.sqrt(dx * dx + dy * dy + dz * dz)

class DisjointSet(val n: Int) {
  val parent: Array[Int] = (0 to n).toArray
  val rank: Array[Int] = Array.fill(n)(0)

  def Find(x: Int): Int =
    if (parent(x) != x)
      parent(x) = Find(parent(x))
    parent(x)

  def Union(x: Int, y: Int): Unit =
    val rootX = Find(x)
    val rootY = Find(y)
    if (rootX == rootY)
      return
    if (rank(rootX) < rank(rootY))
      parent(rootX) = rootY
    else if (rank(rootX) > rank(rootY))
      parent(rootY) = rootX
    else {
      parent(rootY) = rootX
      rank(rootX) = rank(rootX) + 1
    }

  def SameSet(x: Int, y: Int): Boolean =
    Find(x) == Find(y)
}

def ParseFile(fileName: String): List[Point] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  def parseLine(line: String): Point =
    val lineRegex = """(\d+),(\d+),(\d+)""".r
    val lineMatch = lineRegex.findFirstMatchIn(line)
    Point(lineMatch.get.group(1).toInt, lineMatch.get.group(2).toInt, lineMatch.get.group(3).toInt)

  fileSource
    .getLines()
    .map(parseLine)
    .toList

def Part1(input: List[Point]): Int = {
  val inputMap =
    input
      .zipWithIndex
      .toMap

  val pointPairs =
    for {
      i <- input
      j <- input
      if i != j
    } yield (i,j)

  val sortedPointPairs =
    pointPairs
      .sortBy((p1, p2) => GetDistance(p1, p2))
      .take(10)

  val disjointSet = DisjointSet(input.length)
  for ((pointA, pointB) <- sortedPointPairs) {
    val test1 = inputMap(pointA)
    val test2 = inputMap(pointB)
    disjointSet.Union(inputMap(pointA), inputMap(pointB))
  }

  val parentList =
    for {
      i <- disjointSet.parent.indices
    } yield disjointSet.Find(i)

  val parentCount =
    parentList
      .groupBy(identity)


  ???
}

def Part2(): Int =
  ???