
import Day4.*

class Day4Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val input = Day4.parseFile("/Day4/TestInput1.txt")
    val obtained = Part1(input)
    val expected = 18
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val input = Day4.parseFile("/Day4/Input.txt")
    val obtained = Part1(input)
    val expected = 2633
    assertEquals(obtained, expected)
  }

  test("Part2") {
    assertEquals(true, false)
    //val input = Day<x>.parseFile("/Day<x>/Input.txt")
    //val obtained = Part2()
    //val expected =
    //assertEquals(obtained, expected)
  }