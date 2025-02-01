
import Day2.*

class Day2Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val input = Day2.parseFile("/Day2/TestInput1.txt")
    val obtained = Part1(input)
    val expected = 2
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val input = Day2.parseFile("/Day2/Input.txt")
    val obtained = Part1(input)
    val expected = 670
    assertEquals(obtained, expected)
  }

  test("Part2 TestInput") {
    val input = Day2.parseFile("/Day2/TestInput1.txt")
    val obtained = Part2(input)
    val expected = 4
    assertEquals(obtained, expected)
  }

  test("Part2") {
    val input = Day2.parseFile("/Day2/Input.txt")
    val obtained = Part2(input)
    val expected = 700
    assertEquals(obtained, expected)
  }