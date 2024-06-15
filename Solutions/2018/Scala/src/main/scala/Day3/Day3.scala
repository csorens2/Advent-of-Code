package Day3

import scala.annotation.tailrec
import scala.io.Source

class FabricSquare(val ID: Int, val xPos: Int, val yPos: Int, val width: Int, val height: Int)

def parseFile(fileName: String): List[FabricSquare] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  val lineRegex = "#(\\d+) @ (\\d+),(\\d+): (\\d+)x(\\d+)".r

  val parseFabricSquareFunc = (toParse: String) =>
    val possibleMatch = lineRegex.findFirstMatchIn(toParse)
    possibleMatch match
      case None => throw new Exception(s"Unable to parse line $toParse")
      case Some(lineMatch) =>
        FabricSquare(lineMatch.group(1).toInt, lineMatch.group(2).toInt, lineMatch.group(3).toInt, lineMatch.group(4).toInt, lineMatch.group(5).toInt)
  fileSource.getLines().map(parseFabricSquareFunc).toList

@tailrec
def fabricCovering(remainingFabric: List[FabricSquare], fabricMap: Map[(Int, Int), Int]): Map[(Int, Int), Int] =
  if remainingFabric.isEmpty then
    fabricMap
  else
    val nextFabric = remainingFabric.head
    val nextFabricMap =
      (nextFabric.xPos until nextFabric.xPos + nextFabric.width).foldLeft(fabricMap)((innerMap, xCoord) =>
        (nextFabric.yPos until nextFabric.yPos + nextFabric.height).foldLeft(innerMap)((innerInnerMap, yCoord) =>
          val nextCoordinateCount =
            innerInnerMap.get((xCoord, yCoord)) match
              case None => 1
              case Some(prevCount) => prevCount + 1
          innerInnerMap + ((xCoord, yCoord) -> nextCoordinateCount)
        ))
    fabricCovering(remainingFabric.tail, nextFabricMap)


def Part1(input: List[FabricSquare]): Int =
  val fabricMap = fabricCovering(input, Map.empty)
  fabricMap.count((_, coverCount) => coverCount > 1)

def Part2(input: List[FabricSquare]): Int =
  val fabricMap = fabricCovering(input, Map.empty)

  @tailrec
  def recPart2(remainingFabricSquares: List[FabricSquare]): Int =
    if remainingFabricSquares.isEmpty then
      throw new Exception("Unable to find uncovered fabric square")
    else
      // For each of the fabric squares, we need to check if they have an overlap spot
      // So we use a nested "find" to scan over each part of the fabric,
      // and if we find an overlap, we pass up a "found" message by returning the pos in the square
      // as an option.
      val nextFabricSquare = remainingFabricSquares.head
      val foundDoubleCoveredSpot =
        (nextFabricSquare.xPos until nextFabricSquare.xPos + nextFabricSquare.width).find(xPos =>
          val innerFoundDoubleCover = (nextFabricSquare.yPos until nextFabricSquare.yPos + nextFabricSquare.height).find(yPos =>
            val checkSpotDoubleCovered = fabricMap.get((xPos, yPos))
            checkSpotDoubleCovered match
              case None => false
              case Some(spotCoverings) if spotCoverings > 1 => true
              case Some(_) => false)
          innerFoundDoubleCover match
            case None => false
            case Some(_) => true
        )
      if foundDoubleCoveredSpot.isEmpty then
        nextFabricSquare.ID
      else
        recPart2(remainingFabricSquares.tail)

  recPart2(input)


