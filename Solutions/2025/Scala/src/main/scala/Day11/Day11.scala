package Day11

import scala.io.Source

case class Device(Name: String, Outputs: List[String])

def ParseFile(fileName: String): List[Device] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  def ParseLine(line: String): Device =
    val lineRegex = "(\\w+): (.+)".r
    val lineMatch = lineRegex.findFirstMatchIn(line)
    Device(lineMatch.get.group(1), lineMatch.get.group(2).split(' ').toList)

  fileSource
    .getLines()
    .map(ParseLine)
    .toList

def Part1(input: List[Device]): Int = {
  // From the problem description, we know the graph is acyclic
  val inputGraph =
    input
      .map(device => (device.Name, device.Outputs))
      .toMap

  def buildDeviceMap(memoizationMap: Map[String, Int], currNode: String): Map[String, Int] =
    if memoizationMap.contains(currNode) then
      memoizationMap
    else
      def foldSubGraphs(accMap: Map[String, Int], nextNode: String): Map[String, Int] =
        val subGraph = buildDeviceMap(accMap, nextNode)
        val toAdd = subGraph(nextNode)
        subGraph.get(currNode) match
          case None => subGraph + (currNode -> toAdd)
          case Some(prevValue) => subGraph + (currNode -> (toAdd + prevValue))

      inputGraph(currNode).foldLeft(memoizationMap)(foldSubGraphs)

  val root = "you"
  val baseMap = Map("out" -> 1)

  buildDeviceMap(baseMap, root)(root)
}

def Part2(): Int =
  ???