import Day1.Day1;
import org.junit.jupiter.api.Test;

import static org.junit.Assert.assertEquals;

class Day1Tests {

    @Test
    void Part1TestInput() {
        var day1 = new Day1();
        var input = day1.ParseInput("Day1/TestInput.txt");
        var result = day1.Part1(input);
        assertEquals(3, result);
    }

    @Test
    void Part1() {
        var day1 = new Day1();
        var input = day1.ParseInput("Day1/Input.txt");
        var result = day1.Part1(input);
        assertEquals(969, result);
    }

    @Test
    void Part2TestInput() {
        var day1 = new Day1();
        var input = day1.ParseInput("Day1/TestInput.txt");
        var result = day1.Part2(input);
        assertEquals(6, result);
    }

    @Test
    void Part2() {
        var day1 = new Day1();
        var input = day1.ParseInput("Day1/Input.txt");
        var result = day1.Part2(input);
        assertEquals(5887, result);
    }
}