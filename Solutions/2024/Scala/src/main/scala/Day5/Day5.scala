package Day5

import scala.io.Source

def parseFile(fileName: String): (List[(Int, Int)], List[List[Int]]) =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  def parsePair(line: String): (Int, Int) =
    val lineRegex = "(\\d+)\\|(\\d+)".r
    val lineMatch = lineRegex.findFirstMatchIn(line)
    (lineMatch.get.group(1).toInt, lineMatch.get.group(2).toInt)

  def parseList(line: String): List[Int] =
    val lineRegex = "(\\d+)".r
    val lineMatches = lineRegex.findAllMatchIn(line)
    lineMatches
      .map(lineMatch => lineMatch.group(1).toInt)
      .toList

  val fileLines = fileSource.getLines().toList
  val pairs =
    fileLines
      .filter(line => "(\\|)".r.unanchored.matches(line))
      .map(parsePair)
  val lists =
    fileLines
      .filter(line => "(,)".r.unanchored.matches(line))
      .map(parseList)
  (pairs, lists)

def ValidateUpdate(orderingPairs: List[(Int, Int)], toValidate: List[Int]): Boolean =
  val updateMap = toValidate.zipWithIndex.toMap
  orderingPairs
    .forall((left, right) =>
      if !updateMap.contains(left) || !updateMap.contains(right) then
        true
      else
        updateMap(left) < updateMap(right))

def Part1(input1: List[(Int, Int)], input2: List[List[Int]]): Int =
  input2
    .filter(update => ValidateUpdate(input1, update))
    .map(update => update(update.length / 2))
    .sum

def Part2(input1: List[(Int, Int)], input2: List[List[Int]]): Int =
  def sortFunction(int1: Int, int2: Int): Boolean =
    val foundPair = input1.find(toFind => toFind == (int1, int2) || toFind == (int2, int1))
    foundPair match
      case None => throw Exception(s"Unable to find pair $int1,$int2")
      case Some((left, right)) =>
        if left == int1 then
          true
        else
          false

  input2
    .filter(update => !ValidateUpdate(input1, update))
    .map(update => update.sortWith(sortFunction))
    .map(update => update(update.length / 2))
    .sum