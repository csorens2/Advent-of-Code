package Day1;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.nio.charset.StandardCharsets;
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

    public List<Rotation> ParseInput(String filepath) {
        var input = getClass().getClassLoader().getResourceAsStream(filepath);
        var lines = (new BufferedReader(new InputStreamReader(input, StandardCharsets.UTF_8))).lines().toList();

        return lines.stream()
                .map(this::ProcessLine)
                .toList();
    }

    public int Part1(List<Rotation> input) {
        var pos = 50;
        var zeroCount = 0;

        for (Rotation rotation : input) {
            int finalAmount = rotation.amount() % 100;
            if (rotation.direction() == Left) {
                if (finalAmount > pos) {
                    pos = 100 - (finalAmount - pos);
                }
                else {
                    pos -= finalAmount;
                }
            }
            else {
                if (pos + finalAmount > 99) {
                    pos = (finalAmount + pos) - 100;
                }
                else {
                    pos += finalAmount;
                }
            }

            if (pos == 0) {
                zeroCount++;
            }
        }

        return zeroCount;
    }

    public int Part2(List<Rotation> input) {
        var pos = 50;
        var zeroCount = 0;

        for (Rotation rotation : input) {
            int finalAmount = rotation.amount();
            while (finalAmount > 100) {
                finalAmount -= 100;
                zeroCount++;
            }
            if (rotation.direction() == Left) {
                if (finalAmount > pos ) {
                    if (pos != 0) {
                        zeroCount++;
                    }

                    pos = 100 - (finalAmount - pos);
                }
                else if (finalAmount == pos) {
                    zeroCount++;
                    pos = 0;
                } else {
                    pos -= finalAmount;
                }
            }
            else {
                if (pos + finalAmount > 99) {
                    zeroCount++;
                    pos = (finalAmount + pos) - 100;
                } else {
                    pos += finalAmount;
                }
            }
        }

        return zeroCount;
    }
}
