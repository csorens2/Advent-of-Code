
import Day8.*

class Day8Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val input = Day8.ParseFile("/Day8/TestInput.txt")
    val obtained = Part1(input, 10)
    val expected = 40
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val input = Day8.ParseFile("/Day8/Input.txt")
    val obtained = Part1(input, 1000)
    val expected = 80446
    assertEquals(obtained, expected)
  }

  test("Part2 TestInput") {
    val input = Day8.ParseFile("/Day8/TestInput.txt")
    val obtained = Part2(input)
    val expected = 25272
    assertEquals(obtained, expected)
  }

  test("Part2") {
    val input = Day8.ParseFile("/Day8/Input.txt")
    val obtained = Part2(input)
    val expected = 51294528
    assertEquals(obtained, expected)
  }