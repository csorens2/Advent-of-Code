import Day2.Day2;
import org.junit.jupiter.api.Test;
import static org.junit.Assert.assertEquals;

class DayXTests {

    @Test
    void Part1TestInput() {
        var day = new Day2();
        var input = day.ParseInput("Day2/TestInput.txt");
        var result = day.Part1(input);
        assertEquals(1227775554L, result);
    }

    @Test
    void Part1() {
        var day = new Day2();
        var input = day.ParseInput("Day2/Input.txt");
        var result = day.Part1(input);
        assertEquals(32976912643L, result);
    }

    @Test
    void Part2TestInput() {
        var day = new Day2();
        var input = day.ParseInput("Day2/TestInput.txt");
        var result = day.Part2(input);
        assertEquals(4174379265L, result);
    }

    @Test
    void Part2() {
        var day = new Day2();
        var input = day.ParseInput("Day2/Input.txt");
        var result = day.Part2(input);
        assertEquals(54446379122L, result);
    }
}