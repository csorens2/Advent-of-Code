import Day5.*

class Day5Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val input = Day5.ParseFile("/Day5/TestInput.txt")
    val obtained = Part1(input)
    val expected = 3L
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val input = Day5.ParseFile("/Day5/Input.txt")
    val obtained = Part1(input)
    val expected = 681L
    assertEquals(obtained, expected)
  }

  test("Part2 TestInput") {
    val input = Day5.ParseFile("/Day5/TestInput.txt")
    val obtained = Part2(input)
    val expected = 14L
    assertEquals(obtained, expected)
  }

  test("Part2") {
    val input = Day5.ParseFile("/Day5/Input.txt")
    val obtained = Part2(input)
    val expected = 348820208020395L
    assertEquals(obtained, expected)
  }