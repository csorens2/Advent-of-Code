import Day9.*

class Day9Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val input = Day9.ParseFile("/Day9/TestInput.txt")
    val obtained = Part1(input)
    val expected = 1928L
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val input = Day9.ParseFile("/Day9/Input.txt")
    val obtained = Part1(input)
    val expected = 6390180901651L
    assertEquals(obtained, expected)
  }

  test("Part2 TestInput") {
    val input = Day9.ParseFile("/Day9/TestInput.txt")
    val obtained = Part2(input)
    val expected = 2858L
    assertEquals(obtained, expected)
  }

  test("Part2") {
    val input = Day9.ParseFile("/Day9/Input.txt")
    val obtained = Part2(input)
    val expected = 6412390114238L
    assertEquals(obtained, expected)
  }