import Day1.*

class Day1Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val (input1, input2) = Day1.parseFile("/Day1/TestInput1.txt")
    val obtained = Part1(input1, input2)
    val expected = 11
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val (input1, input2) = Day1.parseFile("/Day1/Input.txt")
    val obtained = Part1(input1, input2)
    val expected = 1970720
    assertEquals(obtained, expected)
  }

  test("Part2 TestInput") {
    val (input1, input2) = Day1.parseFile("/Day1/TestInput1.txt")
    val obtained = Part2(input1, input2)
    val expected = 31
    assertEquals(obtained, expected)
  }

  test("Part2") {
    val (input1, input2) = Day1.parseFile("/Day1/Input.txt")
    val obtained = Part2(input1, input2)
    val expected = 17191599
    assertEquals(obtained, expected)
  }