package Day9

import scala.io.Source

enum 

def ParseFile(fileName: String): Array[Int] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  fileSource
    .getLines()
    .next()
    .map(intChar => intChar - '0')
    .toArray


def Part1(Array[Int]): Int =
  ???

def Part2(): Int =
  ???