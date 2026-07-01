package Day11

import scala.io.Source

def ParseFile(fileName: String): Map[String, List[String]] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  def ParseLine(line: String): (String, List[String]) =
    val lineRegex = "(\\w+): (.+)".r
    val lineMatch = lineRegex.findFirstMatchIn(line)
    (lineMatch.get.group(1), lineMatch.get.group(2).split(' ').toList)

  fileSource
    .getLines()
    .map(ParseLine)
    .toMap()

def GetNumPaths[A](graphMap: Map[A, List[A]]) (baseCaseMemoizationMap: Map[A, Long], startNode: A): Long =
  def buildNumPathsMap(memoizationMap: Map[A, Long], currNode: A): Map[A, Long] =
    if memoizationMap.contains(currNode) then
      memoizationMap
    else
      def foldSubGraphs(accMap: Map[A, Long], nextNode: A): Map[A, Long] =
        val subGraph = buildNumPathsMap(accMap, nextNode)
        val toAdd = subGraph(nextNode)
        subGraph.get(currNode) match
          case None => subGraph + (currNode -> toAdd)
          case Some(prevValue) => subGraph + (currNode -> (toAdd + prevValue))

      graphMap(currNode)
        .foldLeft(memoizationMap)(foldSubGraphs)

  buildNumPathsMap(baseCaseMemoizationMap, startNode)(startNode)

def Part1(input: Map[String, List[String]]): Long =
  GetNumPaths(input)(Map.empty + ("out" -> 1L), "you")

def Part2(input: Map[String, List[String]]): Long =

  val svr = "svr"
  val dac = "dac"
  val fft = "fft"
  val out = "out"

  val defaultBaseCaseMap = Map.empty + (out -> 0L)

  val numPathsFunc = GetNumPaths(input)

  val SVR_to_FFT = numPathsFunc(defaultBaseCaseMap + (fft -> 1L), svr)
  val FFT_to_DAC = numPathsFunc(defaultBaseCaseMap + (dac -> 1L), fft)
  val DAC_to_OUT = numPathsFunc(defaultBaseCaseMap + (out -> 1L), dac)
  val numA = SVR_to_FFT * FFT_to_DAC * DAC_to_OUT

  val SVR_to_DAC = numPathsFunc(defaultBaseCaseMap + (dac -> 1L), svr)
  val DAC_to_FFT = numPathsFunc(defaultBaseCaseMap + (fft -> 1L), dac)
  val FFT_to_OUT = numPathsFunc(defaultBaseCaseMap + (out -> 1L), fft)
  val numB = SVR_to_DAC * DAC_to_FFT * FFT_to_OUT

  numA + numB