package Day4

import scala.io.Source

class Point(val y: Int, val x: Int):
  def Value(using map: Array[Array[Char]]): Char =
    map(y)(x)
  def InBounds(using map: Array[Array[Char]]): Boolean =
    if y < 0 || x < 0 then
      false
    else if y >= map.length || x >= map(0).length then
      false
    else
      true
end Point

def parseFile(fileName: String): Array[Array[Char]] =
  val resource = Source.getClass.getResource(fileName)
  val fileSource = Source.fromFile(resource.toURI)

  fileSource
    .getLines()
    .map(line => line.toArray)
    .toArray

def Part1(input: Array[Array[Char]]): Int =
  implicit val implicitInput: Array[Array[Char]] = input

  def processXmas(currPoint: Point, chainLength: Int, movementFunc: Point => Point): Boolean =
    if !currPoint.InBounds then
      false
    else
      chainLength match
        case 0 if currPoint.Value == 'X' => processXmas(movementFunc(currPoint), chainLength + 1, movementFunc)
        case 1 if currPoint.Value == 'M' => processXmas(movementFunc(currPoint), chainLength + 1, movementFunc)
        case 2 if currPoint.Value == 'A' => processXmas(movementFunc(currPoint), chainLength + 1, movementFunc)
        case 3 if currPoint.Value == 'S' => true
        case _ => false

  def processPoint(point: Point): Int =
    if point.Value != 'X' then
      throw new Exception("Invalid point to process")
    else
      val movementFuncs: List[Point => Point] =
        List(
          (point: Point) => Point(point.y - 1, point.x),
          (point: Point) => Point(point.y - 1, point.x + 1),
          (point: Point) => Point(point.y, point.x + 1),
          (point: Point) => Point(point.y + 1, point.x + 1),
          (point: Point) => Point(point.y + 1, point.x),
          (point: Point) => Point(point.y + 1, point.x - 1),
          (point: Point) => Point(point.y, point.x - 1),
          (point: Point) => Point(point.y - 1, point.x - 1))

      movementFuncs
        .map(moveFunc => processXmas(point, 0, moveFunc))
        .count(_ == true)

  val numXmas =
    for
      y <- input.indices
      x <- input(0).indices
      if Point(y,x).Value == 'X'
    yield
      processPoint(Point(y,x))
  numXmas.sum

def Part2(input: Array[Array[Char]]): Int =
  ???