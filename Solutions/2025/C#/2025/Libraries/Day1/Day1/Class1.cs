using System.Security.AccessControl;
using System.Text.RegularExpressions;
using static Direction;

public enum Direction
{
    Left,
    Right
}

namespace Day1
{
    
    public static class Day1
    {
        

        public record Rotation(Direction direction, int amount);

        public static Rotation ParseLine(string line)
        {
            var regexString = @"([L|R])(\d+)";
            var match = Regex.Match(line, regexString);

            var amount = int.Parse(match.Groups[2].Value);

            if (match.Groups[1].Value == "R")
            {
                return new Rotation(Right, amount);
            }
            else
            {
                return new Rotation(Left, amount);
            }
        }

        public static List<Rotation> ParseInput(string filepath)
        {
            return 
                File.ReadAllLines(filepath)
                    .Select(line => ParseLine(line))
                    .ToList();
        }
    }
}
