import Day3.*

class Day3Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val input = Day3.parseFile("/Day3/TestInput1.txt")
    val obtained = Part1(input)
    val expected = 161
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val input = Day3.parseFile("/Day3/Input.txt")
    val obtained = Part1(input)
    val expected = 184122457
    assertEquals(obtained, expected)
  }

  test("Part2 TestInput") {
    val input = Day3.parseFile("/Day3/TestInput2.txt")
    val obtained = Part2(input)
    val expected = 48
    assertEquals(obtained, expected)
  }

  test("Part2") {
    val input = Day3.parseFile("/Day3/Input.txt")
    val obtained = Part2(input)
    val test = 1
    //val expected =
    //assertEquals(obtained, expected)
  }