import Day11.*

class Day11Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val input = Day11.ParseFile("/Day11/TestInput1.txt")
    val obtained = Part1(input)
    val expected = 5L
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val input = Day11.ParseFile("/Day11/Input.txt")
    val obtained = Part1(input)
    val expected = 523L
    assertEquals(obtained, expected)
  }

  test("Part2 TestInput") {
    val input = Day11.ParseFile("/Day11/TestInput2.txt")
    val obtained = Part2(input)
    val expected = 2L
    assertEquals(obtained, expected)
  }

  test("Part2") {
    val input = Day11.ParseFile("/Day11/Input.txt")
    val obtained = Part2(input)
    val expected = 517315308154944L
    assertEquals(obtained, expected)
  }