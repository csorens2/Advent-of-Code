package Day1;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Collections;
import java.util.List;

enum Direction {
    Left,
    Right
}

record Rotation(Direction direction, int amount) {}

public class Day1 {

    public List<Rotation> ParseInput(String filepath) throws IOException {
        var input = getClass().getClassLoader().getResourceAsStream(filepath);
        var lines = (new BufferedReader(new InputStreamReader(input, StandardCharsets.UTF_8))).lines().toList();

        return Collections.emptyList();
    }

    public int Part1() {
        return 1;
    }

    public int Part2() {
        return 1;
    }
}
