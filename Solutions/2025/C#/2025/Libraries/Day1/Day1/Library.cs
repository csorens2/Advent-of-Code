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

        public static int Part1(List<Rotation> input)
        {
            var pos = 50;
            var zeroCount = 0;

            foreach(Rotation nextRotation in input)
            {
                var finalRotation = nextRotation.amount % 100;

                if (nextRotation.direction == Left)
                {
                    if (finalRotation > pos)
                    {
                        pos = 100 - (finalRotation - pos);
                    }
                    else
                    {
                        pos = pos - finalRotation;
                    }
                }
                else
                {
                    if (pos + finalRotation > 99)
                    {
                        pos = (pos + finalRotation) - 100;
                    }
                    else
                    {
                        pos = pos + finalRotation;
                    }
                }

                if(pos == 0)
                {
                    zeroCount++;
                }

            }

            return zeroCount;
        }

        public static int Part2(List<Rotation> input)
        {
            var pos = 50;
            var zeroCount = 0;

            foreach(Rotation rotation in input)
            {
                var finalAmount = rotation.amount;
                if (finalAmount > 100)
                {
                    finalAmount -= 100;
                    zeroCount++;
                }

                if (rotation.direction == Left)
                {
                    if (finalAmount > pos)
                    {
                        if (pos != 0)
                        {
                            zeroCount++;
                        }

                        pos = 100 - (finalAmount - pos);
                    }
                    else if (finalAmount == pos)
                    {
                        zeroCount++;
                        pos = 0;
                    }
                    else
                    {
                        pos -= finalAmount;
                    }
                }
                else
                {
                    if (pos + finalAmount > 99)
                    {
                        zeroCount++;
                        pos = (finalAmount + pos) - 100;
                    }
                    else
                    {
                        pos += finalAmount;
                    }
                }
            }

            return zeroCount;
        }
    }
}
