package Day2;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.regex.Pattern;

public class Day2 {

    record Range(Long left, Long right) {}

    public Range ParsePair(String rawPair) {
        var regex = "(\\d+)-(\\d+)";
        var pattern = Pattern.compile(regex);
        var matcher = pattern.matcher(rawPair);

        matcher.find();

        return new Range(Long.parseLong(matcher.group(1)), Long.parseLong(matcher.group(2)));
    }

    public List<Range> ParseInput(String filepath) {
        var input = getClass().getClassLoader().getResourceAsStream(filepath);
        var line = (new BufferedReader(new InputStreamReader(input, StandardCharsets.UTF_8))).lines().toList().getFirst();

        return
                Arrays.stream(line.split(","))
                .map(this::ParsePair)
                .toList();
    }

    public long Part1(List<Range> input) {
        List<String> invalidIds = new ArrayList<>();

        for(Range nextRange : input) {
            for(long i = nextRange.left; i <= nextRange.right; i++) {
                var numString = String.valueOf(i);
                var numLength = numString.length();
                if (numLength % 2 != 0) {
                    continue;
                }

                var leftString = numString.substring(0, numLength / 2);
                var rightString = numString.substring(numLength / 2);
                if (leftString.equals(rightString)) {
                    invalidIds.add(numString);
                }

            }
        }

        return invalidIds.stream()
                .map(Long::parseLong)
                .mapToLong(Long::longValue).sum();
    }

    public boolean IsInvalidPart2(Long toCheck) {
        var numString = String.valueOf(toCheck);
        var numLength = numString.length();

        for(int chunkLength = 1; chunkLength < numLength; chunkLength++){
            if (numLength % chunkLength != 0) {
                continue;
            }

            var baseChunk = numString.substring(0, chunkLength);
            var remainingString = numString.substring(chunkLength);
            while (!remainingString.isEmpty()) {
                var nextChunk = remainingString.substring(0,chunkLength);
                if(!baseChunk.equals(nextChunk)) {
                    break;
                }

                remainingString = remainingString.substring(chunkLength);
            }

            if (remainingString.isEmpty()) {
                return true;
            }
        }

        return false;
    }

    public long Part2(List<Range> input) {

        List<Long> invalidIds = new ArrayList<Long>();

        for(Range nextRange : input) {
            for(long i = nextRange.left; i <= nextRange.right; i++) {
                if (IsInvalidPart2(i)) {
                    invalidIds.add(i);
                }
            }
        }

        return invalidIds.stream()
                .mapToLong(Long::longValue).sum();
    }
}
