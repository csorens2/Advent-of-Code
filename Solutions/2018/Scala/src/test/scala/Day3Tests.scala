import Day3.*

class Day3Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val input = Day3.parseFile("/Day3/TestInput1.txt")
    val obtained = Part1(input)
    val expected = 4
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val input = Day3.parseFile("/Day3/Input.txt")
    val obtained = Part1(input)
    val expected = 103806
    assertEquals(obtained, expected)
  }

  test("Part2 TestInput") {
    val input = Day3.parseFile("/Day3/TestInput1.txt")
    val obtained = Part2(input)
    val expected = 3
    assertEquals(obtained, expected)
  }

  test("Part2") {
    val input = Day3.parseFile("/Day3/Input.txt")
    val obtained = Part2(input)
    val expected = 625
    assertEquals(obtained, expected)
  }