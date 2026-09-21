import Day10.*

class Day10Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val input = Day10.ParseFile("/Day10/TestInput.txt")
    val obtained = Part1(input)
    val expected = 7
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val input = Day10.ParseFile("/Day10/Input.txt")
    val obtained = Part1(input)
    val expected = 415
    assertEquals(obtained, expected)
  }

  test("Part2 TestInput") {
    val input = Day10.ParseFile("/Day10/TestInput.txt")
    val obtained = Part2(input)
    val expected = 33
    assertEquals(obtained, expected)
  }

  test("Part2") {
    val input = Day10.ParseFile("/Day10/Input.txt")
    val obtained = Part2(input)
    val expected = 0
    assertEquals(obtained, expected)
  }