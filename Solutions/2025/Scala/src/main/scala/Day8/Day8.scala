package Day8

import scala.io.Source

case class Point(X: Int, Y: Int, Z: Int)

/**
 * We need to use longs in this function to deal with numbers too big for doubles to handle.
 * As a result, we can't use sqrt since there isn't a version of it for longs.
 * However, since the distance between two points is only used to sort point-pairs,
 * the square of the distance will also work, so we return said square of distance.
  */
def GetDistanceSquared(p1: Point, p2: Point): Long =
  val dx = (p2.X - p1.X).toLong
  val dy = (p2.Y - p1.Y).toLong
  val dz = (p2.Z - p1.Z).toLong
  dx * dx + dy * dy + dz * dz

class DisjointSet(val n: Int) {
  private val parent: Array[Int] = (0 until n).toArray
  private val count: Array[Int] = Array.fill(n)(1)

  def Find(x: Int): Int =
    if(parent(x) == x)
      x
    else
      Find(parent(x))

  def Union(x: Int, y: Int): Unit =
    val rootX = Find(x)
    val rootY = Find(y)

    if (rootX == rootY)
      return

    val newCount = count(rootX) + count(rootY)
    // We always want to join the smaller set into the bigger set for efficiency
    if (count(rootX) < count(rootY)) {
      parent(rootX) = rootY
      count(rootY) = newCount
    } else if (count(rootX) > count(rootY)) {
      parent(rootY) = rootX
      count(rootX) = newCount
    } else { // Counts are equal, so the choice of root is arbitrary.
      parent(rootY) = rootX
      count(rootX) = newCount
    }

  def SameSet(x: Int, y: Int): Boolean =
    Find(x) == Find(y)

  def GetCount(x: Int): Int =
    count(Find(x))
}

def ParseFile(fileName: String): Array[Point] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  def parseLine(line: String): Point =
    val lineRegex = """(\d+),(\d+),(\d+)""".r
    val lineMatch = lineRegex.findFirstMatchIn(line)
    Point(lineMatch.get.group(1).toInt, lineMatch.get.group(2).toInt, lineMatch.get.group(3).toInt)

  fileSource
    .getLines()
    .map(parseLine)
    .toArray

def GetSortedPointPairs(pointArray: Array[Point]): List[(Point, Point)] =
  var pointPairs = List.empty[(Point, Point)]
  for (i <- pointArray.indices) {
    for (j <- i + 1 until pointArray.length) {
      pointPairs = (pointArray(i), pointArray(j)) :: pointPairs
    }
  }
  pointPairs.sortBy((p1, p2) => GetDistanceSquared(p1, p2))

def GetPointToDisjointSetIndexMap(input: Array[Point]): Map[Point, Int] =
  input
    .zipWithIndex
    .toMap

def Part1(input: Array[Point], numConnections: Int): Int = {
  if (input.length < 3)
    throw Exception("Part 1 requires a minimum of 3 objects")

  val sortedPointPairs = GetSortedPointPairs(input)
  val pointToDisjointSetIndexMap = GetPointToDisjointSetIndexMap(input)
  val disjointSet = DisjointSet(input.length)

  for ((pointA, pointB) <- sortedPointPairs.take(numConnections)) {
    disjointSet.Union(pointToDisjointSetIndexMap(pointA), pointToDisjointSetIndexMap(pointB))
  }

  pointToDisjointSetIndexMap
    .values
    .toList
    .map(disjointSetIndex => disjointSet.Find(disjointSetIndex))
    .distinct
    .map(rootDisjointSetIndex => disjointSet.GetCount(rootDisjointSetIndex))
    .sorted(Ordering[Int].reverse)
    .take(3)
    .product
}

def Part2(input: Array[Point]): Int =
  if (input.length < 2)
    throw Exception("Requires minimum 2 objects")

  val sortedPointPairs = GetSortedPointPairs(input)
  val pointToDisjointSetIndexMap = GetPointToDisjointSetIndexMap(input)
  val disjointSet = DisjointSet(input.length)

  var currentHead: (Point,Point) = null
  var currentList = sortedPointPairs
  var lastSetCount = 0
  while (lastSetCount != input.length) {
    currentHead = currentList.head

    val (currentHeadA, currentHeadB) = currentHead
    val setNumA = pointToDisjointSetIndexMap(currentHeadA)
    val setNumB = pointToDisjointSetIndexMap(currentHeadB)
    disjointSet.Union(setNumA, setNumB)

    lastSetCount = disjointSet.GetCount(setNumA)
    currentList = currentList.tail
  }

  val (finalA, finalB) = currentHead
  finalA.X * finalB.X