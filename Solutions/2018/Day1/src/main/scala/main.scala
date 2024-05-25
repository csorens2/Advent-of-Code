
import scala.io.Source

enum Sign:
  case Plus, Minus

class FrequencyChange(val frequencySign: Sign, val frequencyValue: Int)

def parseFile(filePath: String): Seq[FrequencyChange] = {
  val fileSource = Source.fromFile(filePath)
  val lineRegex = "([+-])(\\d+)".r

  val parseFreqChange = (toParse: String) =>
    val possibleMatch = lineRegex.findFirstMatchIn(toParse)
    possibleMatch match {
      case Some(lineMatch) =>
        val freqVal = lineMatch.group(1).toInt
        lineMatch.group(0) match
          case "+" => FrequencyChange(Sign.Plus, freqVal)
          case "-" => FrequencyChange(Sign.Minus, freqVal)
          case _ => throw new Exception("Unknown sign parsed")
      case None => throw new Exception("Line match not found for: " + toParse)
    }

  fileSource.getLines().map(parseFreqChange).toList
}

def Part1(freqChanges: Seq[FrequencyChange]): Int = {
  freqChanges.headOption match {
    case Some(headFreqChange) =>
      val sign = headFreqChange.frequencySign match {
        case Sign.Plus => 1
        case Sign.Minus => -1
        case _ => throw new Exception("Unknown sign parsed")
      }
      (sign * headFreqChange.frequencyValue) + Part1(freqChanges.tail)
    case None => 0
  }
}

@main
def main(): Unit = {
  val input = parseFile("Input.txt")
  val part1Result = Part1(input)
  println("Part1 Result: " + part1Result)
}
