import Day1.Day1;
import org.junit.jupiter.api.Test;

import java.io.IOException;

import static org.junit.jupiter.api.Assertions.assertEquals;

class Day1Tests {

    @Test
    void Part1TestInput() throws IOException {
        var day1 = new Day1();
        var input = day1.ParseInput("Day1/TestInput.txt");
    }
}