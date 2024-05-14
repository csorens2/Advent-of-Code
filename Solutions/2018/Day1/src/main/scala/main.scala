import scala.io.Source

enum Sign:
  case Plus, Minus

class FrequencyChange(val frequencySign: Sign, val frequencyValue: Int)

def parseFile(filePath: String) = {
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



  for (line <- fileSource.getLines()) {
  } yield parseFreqChange(line)
}

@main
def main(): Unit = {
  println("Hello worlds!")
}
