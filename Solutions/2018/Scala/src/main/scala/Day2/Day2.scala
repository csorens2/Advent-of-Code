package Day2

import scala.annotation.tailrec
import scala.io.Source

def parseFile(fileName: String): List[String] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)
  fileSource.getLines().toList

def Part1(input: List[String]): Int =
  def processString(charMap: Map[Char, Int], remainingString: List[Char]): (Boolean, Boolean) =
    remainingString.headOption match
      case None =>
        val hasDouble = charMap.count((_, count) => count == 2) > 0
        val hasTriple = charMap.count((_, count) => count == 3) > 0
        (hasDouble, hasTriple)
      case Some(nextChar) =>
        val nextCharCount =
          charMap.get(nextChar) match
            case None => 1
            case Some(prevCount) => prevCount + 1
        processString(charMap + (nextChar -> nextCharCount), remainingString.tail)

  val processedStrings = input.map((inputString => processString(Map.empty, inputString.toList)))
  val doubleCount = processedStrings.count((double, _) => double)
  val tripleCount = processedStrings.count((_, triple) => triple)
  doubleCount * tripleCount

def Part2(input: List[String]): String = ???