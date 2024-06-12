package Day1

@main
def main(): Unit =
  println("Hello World from Day1!")
  val input = parseFile("/Input.txt")
  val part1Result = Part1(input)
  println(s"Part 1 Result: $part1Result")

/*
object Main extends App:
  println("Hello World from Day1!")

 */
