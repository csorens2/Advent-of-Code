
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
    val expected = 0
    assertEquals(obtained, expected)
  }

  test("Part2") {
    assertEquals(true, false)
    //val input = Day<x>.parseFile("/Day<x>/Input.txt")
    //val obtained = Part2()
    //val expected =
    //assertEquals(obtained, expected)
  }