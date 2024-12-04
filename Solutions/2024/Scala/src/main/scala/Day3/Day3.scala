package Day3

import scala.io.Source
import scala.util.matching.Regex.Match


// Sealed trait of case classes is the closest to Disjoint Sets that Scala supports
sealed trait Command

case class Do() extends Command
case class Dont() extends Command
case class Mul(mul1: Int, mul2: Int) extends Command

def parseFile(fileName: String): List[List[Command]] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  val regex = "mul\\((\\d+),(\\d+)\\)|don't\\(\\)|do\\(\\)".r

  def parseMatch(toParse: Match): Command =
    val matchString = toParse.matched
    if matchString.contains("mul") then
      Mul(toParse.group(1).toInt, toParse.group(2).toInt)
    else if matchString.contains("don't") then
      Dont()
    else
      Do()

  fileSource
    .getLines()
    .map(line =>
      regex
        .findAllMatchIn(line)
        .map(parseMatch)
        .toList)
    .toList

def Part1(input: List[List[Command]]): Int =
  input
    .map(line =>
      line
        .filter(command => command.isInstanceOf[Mul])
        .map(command => command.asInstanceOf[Mul].mul1 * command.asInstanceOf[Mul].mul2)
        .sum)
    .sum



def Part2(input: List[List[Command]]): Int =

  def processLine(remainingLine: List[Command], active: Boolean): Int =
    if remainingLine.isEmpty then
      0
    else
      remainingLine.head match
        case Do() => processLine(remainingLine.tail, true)
        case Dont() => processLine(remainingLine.tail, false)
        case Mul(val1, val2) if active => (val1 * val2) + processLine(remainingLine.tail, active)
        case _ => processLine(remainingLine.tail, active)



  input
    .map(line => processLine(line, true))
    .sum