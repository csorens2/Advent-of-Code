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
          val nextCoordCount =
            innerInnerMap.get((xCoord, yCoord)) match
              case None => 1
              case Some(prevCount) => prevCount + 1
          innerInnerMap + ((xCoord, yCoord) -> nextCoordCount)
        ))
    fabricCovering(remainingFabric.tail, nextFabricMap)


def Part1(input: List[FabricSquare]): Int =
  val fabricMap = fabricCovering(input, Map.empty)
  fabricMap.count((_, coverCount) => coverCount > 1)

def Part2(input: List[FabricSquare]): Int =
  val fabricMap = fabricCovering(input, Map.empty)
  ???


