package Day1

@main
def main(): Unit =
  println("Hello World from Day1!")
  val input = parseFile("/Input.txt")
  val part1Result = Part1(input)
  println(s"Part 1 Result: $part1Result") // 508
  val part2Result = Part2(input)
  println(s"Part 2 Result: $part2Result") // 549
