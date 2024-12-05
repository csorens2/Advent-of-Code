package Day3

import scala.io.Source
import scala.util.matching.Regex.Match

// Sealed trait of case classes is the closest to Disjoint Sets that Scala supports
sealed trait Command

case class Do() extends Command
case class Dont() extends Command
case class Mul(mul1: Int, mul2: Int) extends Command

def parseFile(fileName: String): List[Command] =
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
    .flatMap(line =>
      regex
        .findAllMatchIn(line)
        .map(parseMatch))
    .toList

def Part1(input: List[Command]): Int =
  input
    .filter(command => command.isInstanceOf[Mul])
    .map(command =>
      val mulCommand = command.asInstanceOf[Mul]
      mulCommand.mul1 * mulCommand.mul2)
    .sum

def Part2(input: List[Command]): Int =

  def processCommands(remainingLine: List[Command], active: Boolean): Int =
    if remainingLine.isEmpty then
      0
    else
      remainingLine.head match
        case Do() =>
          processCommands(remainingLine.tail, true)
        case Dont() =>
          processCommands(remainingLine.tail, false)
        case Mul(val1, val2) if active => (val1 * val2) + processCommands(remainingLine.tail, active)
        case _ => processCommands(remainingLine.tail, active)

  processCommands(input, true)