package Day1;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.nio.charset.StandardCharsets;
import java.util.Collections;
import java.util.List;
import java.util.regex.Pattern;

import static Day1.Direction.Right;
import static Day1.Direction.Left;

enum Direction {
    Left,
    Right
}

record Rotation(Direction direction, int amount) {}

public class Day1 {

    public Rotation ProcessLine(String line) {
        var regex = "([L|R])(\\d+)";
        var pattern = Pattern.compile(regex);
        var matcher = pattern.matcher(line);

        matcher.find();

        Direction finalDirection;
        if (matcher.group(1).equals("R")) {
            finalDirection = Right;
        }
        else {
            finalDirection = Left;
        }

        return new Rotation(finalDirection, Integer.parseInt(matcher.group(2)));
    }

    public List<Rotation> ParseInput(String filepath) throws IOException {
        var input = getClass().getClassLoader().getResourceAsStream(filepath);
        var lines = (new BufferedReader(new InputStreamReader(input, StandardCharsets.UTF_8))).lines().toList();

        return lines.stream()
                .map(this::ProcessLine)
                .toList();
    }

    public int Part1() {
        return 1;
    }

    public int Part2() {
        return 1;
    }
}
