package Day1

import scala.annotation.tailrec
import scala.io.Source

/*
// https://stackoverflow.com/questions/23361212/where-do-i-put-my-resources-in-scala
// Stackoverflow Humans 1 : ChatGPT 0 : Google 0.25
def ReadFile(input: String): Unit =

  val resource = Source.getClass.getResource(input)
  //val fileSource = Source.fromFile(input)
  //println(resourcePath)

  val test2 = Source.fromFile(resource.toURI)

  val test = 1
*/

/*
enum Sign:
  case Plus, Minus

class FrequencyChange(val frequencySign: Sign, val frequencyValue: Int)

def parseFile(fileName: String): Seq[FrequencyChange] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  val lineRegex = "([+-])(\\d+)".r
  val parseFreqChangeFunc = (toParse: String) =>
    val possibleMatch = lineRegex.findFirstMatchIn(toParse)
    possibleMatch match
      case Some(lineMatch) =>
        val freqVal = lineMatch.group(2).toInt
        lineMatch.group(1) match
          case "+" => FrequencyChange(Sign.Plus, freqVal)
          case "-" => FrequencyChange(Sign.Minus, freqVal)
          case _ => throw new Exception("Unknown sign parsed")
      case None => throw new Exception("Line match not found for: " + toParse)
  fileSource.getLines().map(parseFreqChangeFunc).toList

def Part1(freqChanges: Seq[FrequencyChange]): Int =
  freqChanges.headOption match
    case Some(headFreqChange) =>
      val sign = headFreqChange.frequencySign match
        case Sign.Plus => 1
        case Sign.Minus => -1
      (sign * headFreqChange.frequencyValue) + Part1(freqChanges.tail)
    case None => 0
 */

def parseFile(fileName: String): List[Int] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  val lineRegex = "([+-])(\\d+)".r

  val parseFreqChangeFunc = (toParse: String) =>
    val possibleMatch = lineRegex.findFirstMatchIn(toParse)
    possibleMatch match
      case Some(lineMatch) =>
        val freqVal = lineMatch.group(2).toInt
        lineMatch.group(1) match
          case "+" => freqVal
          case "-" => -1 * freqVal
          case _ => throw new Exception("Unknown sign parsed")
      case None => throw new Exception("Line match not found for: " + toParse)
  fileSource.getLines().map(parseFreqChangeFunc).toList

def Part1(input: List[Int]): Int =
  input.sum

def Part2(input: List[Int]): Int =

  // LazyList is the equivalent to a sequence in F#
  def toInfiniteLazyList(intList: List[Int]): LazyList[Int] =
    def loop(xs: List[Int]): LazyList[Int] = xs match
      case Nil => loop(intList)  // Start again from the original list
      case head :: tail => head #:: loop(tail) // #:: === "Lazily append head to the front of the lazy list"
    loop(intList)

  @tailrec
  def recPart2(freqSet: Set[Int], lastFreq: Int, freqChanges: LazyList[Int]): Int =
    freqChanges.headOption match
      case None => throw new Exception("Frequency changes list should be infinite, but isn't.")
      case Some(freqChange) =>
        val currentFreq = lastFreq + freqChange
        if freqSet.contains(currentFreq) then
          currentFreq
        else
          recPart2(freqSet + currentFreq, currentFreq, freqChanges.tail)

  val infiniteSeq = toInfiniteLazyList(input)

  recPart2(Set.empty, 0, infiniteSeq)





