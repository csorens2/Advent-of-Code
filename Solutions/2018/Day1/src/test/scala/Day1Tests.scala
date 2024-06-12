import Day1.*

class Day1Tests extends munit.FunSuite:
  private val input = Day1.parseFile("/Input.txt")

  test("Part1 Test") {
    val obtained = Part1(input)
    val expected = 508
    assertEquals(obtained, expected)
  }