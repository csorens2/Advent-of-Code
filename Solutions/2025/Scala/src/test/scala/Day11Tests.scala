import Day11.*

class Day11Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val input = Day11.ParseFile("/Day11/TestInput1.txt")
    val obtained = Part1(input)
    val expected = 5
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val input = Day11.ParseFile("/Day11/Input.txt")
    val obtained = Part1(input)
    val expected = 523
    //assertEquals(obtained, expected)
  }

  test("Part2") {
    assertEquals(true, false)
    //val input = Day11.ParseFile("/Day11/Input.txt")
    //val obtained = Part2()
    //val expected =
    //assertEquals(obtained, expected)
  }