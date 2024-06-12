package Day2

import scala.annotation.tailrec
import scala.io.Source

def parseFile(fileName: String): List[String] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)
  fileSource.getLines().toList

def Part1(input: List[String]): Int =
  @tailrec
  def checkForDoubleTriple(charMap: Map[Char, Int], remainingString: List[Char]): (Boolean, Boolean) =
    remainingString.headOption match
      case None =>
        val hasDouble = charMap.count((_, count) => count == 2) > 0
        val hasTriple = charMap.count((_, count) => count == 3) > 0
        (hasDouble, hasTriple)
      case Some(nextChar) =>
        val nextMapCharCount =
          charMap.get(nextChar) match
            case None => 1
            case Some(prevCount) => prevCount + 1
        checkForDoubleTriple(charMap + (nextChar -> nextMapCharCount), remainingString.tail)

  val processedStrings = input.map(inputString => checkForDoubleTriple(Map.empty, inputString.toList))
  val doubleCount = processedStrings.count((double, _) => double)
  val tripleCount = processedStrings.count((_, triple) => triple)
  doubleCount * tripleCount

def Part2(input: List[String]): String =
  // Can assume that the string is the same length
  @tailrec
  def singleDiffString(string1: List[Char], string2: List[Char], currCount: Int): Boolean =
    if string1.isEmpty || string2.isEmpty then
      currCount == 1
    else if string1.head == string2.head then
      singleDiffString(string1.tail, string2.tail, currCount)
    else
      singleDiffString(string1.tail, string2.tail, currCount + 1)

  @tailrec
  def findPair(remainingStrings: List[String]): (String, String) =
    remainingStrings.headOption match
      case None => throw new Exception("Did not find pair")
      case Some(nextToCompare) =>
        val foldResult = remainingStrings.tail.foldLeft(None)((foundPair: Option[String], currString: String) =>
          if singleDiffString(nextToCompare.toList, currString.toList, 0) then
            Some(currString)
          else
            foundPair)
        foldResult match
          case None => findPair(remainingStrings.tail)
          case Some(matchingString) => (nextToCompare, matchingString)

  val (singleDiff1, singleDiff2) = findPair(input)

  @tailrec
  def removeDiffChar(string1: List[Char], string2: List[Char], stringChars: List[Char]): String =
    if string1.isEmpty || string2.isEmpty then
      stringChars.mkString
    else if string1.head == string2.head then
      removeDiffChar(string1.tail, string2.tail, stringChars :+ string1.head)
    else
      removeDiffChar(string1.tail, string2.tail, stringChars)

  removeDiffChar(singleDiff1.toList, singleDiff2.toList, List.empty)
