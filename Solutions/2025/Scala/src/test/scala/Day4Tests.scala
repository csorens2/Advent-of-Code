import Day4.*

class Day4Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val input = Day4.ParseFile("/Day4/TestInput.txt")
    val obtained = Part1(input)
    val expected = 13
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val input = Day4.ParseFile("/Day4/Input.txt")
    val obtained = Part1(input)
    val expected = 1508
    assertEquals(obtained, expected)
  }

  test("Part2 TestInput") {
    val input = Day4.ParseFile("/Day4/TestInput.txt")
    val obtained = Part2(input)
    val expected = 43
    assertEquals(obtained, expected)
  }

  test("Part2") {
    val input = Day4.ParseFile("/Day4/Input.txt")
    val obtained = Part2(input)
    val expected = 8538
    assertEquals(obtained, expected)
  }