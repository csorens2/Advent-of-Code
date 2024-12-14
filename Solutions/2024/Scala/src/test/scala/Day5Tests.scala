import Day5.*

class Day5Tests extends munit.FunSuite:

  test("Part1 TestInput") {
    val (input1, input2) = Day5.parseFile("/Day5/TestInput1.txt")
    val obtained = Part1(input1, input2)
    val expected = 143
    assertEquals(obtained, expected)
  }

  test("Part1") {
    val (input1, input2) = Day5.parseFile("/Day5/Input.txt")
    val obtained = Part1(input1, input2)
    val expected = 6267
    assertEquals(obtained, expected)
  }

  test("Part2 TestInput") {
    val (input1, input2) = Day5.parseFile("/Day5/TestInput1.txt")
    val obtained = Part2(input1, input2)
    val expected = 123
    assertEquals(obtained, expected)
  }

  test("Part2") {
    val (input1, input2) = Day5.parseFile("/Day5/Input.txt")
    val obtained = Part2(input1, input2)
    val expected = 5184
    assertEquals(obtained, expected)
  }