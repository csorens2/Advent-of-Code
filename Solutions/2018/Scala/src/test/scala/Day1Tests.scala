import Day1.*

class Day1Tests extends munit.FunSuite:
  private val input = Day1.parseFile("/Day1/Input.txt")

  test("Part1 Test") {
    val obtained = Part1(input)
    val expected = 508
    assertEquals(obtained, expected)
  }

  test("Part2 Test") {
    val obtained = Part2(input)
    val expected = 549
    assertEquals(obtained, expected)
  }