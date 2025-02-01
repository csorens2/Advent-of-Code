package Day4

import scala.io.Source

class Date(val Month: Int, val Day: Int, val Hour: Int)

class Timespan(val Start: Date, val End: Date)

def parseFile(fileName: String): Unit =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  val regex1 = "\\[(\\d+)-(\\d+)-(\\d+) (\\d+):(\\d+)\\] Guard #(\\d+)".r
  val regex2 = "\\[(\\d+)-(\\d+)-(\\d+) (\\d+):(\\d+)\\] falls asleep".r
  val regex3 = "\\[(\\d+)-(\\d+)-(\\d+) (\\d+):(\\d+)\\] wakes up".r

  ???



def Part1(): Int =
  ???

def Part2(): Int =
  ???