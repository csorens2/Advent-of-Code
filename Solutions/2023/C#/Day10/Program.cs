using System;
using System.Collections.Generic;
using System.Linq;

namespace Day10
{
    public enum Tile
    {
        VerticalPipe,
        HorizontalPipe,
        NorthEastBend,
        NorthWestBend,
        SouthEastBend,
        SouthWestBend,
        Ground,
        Start,
    }

    public class Program
    {
        public static List<List<Tile>> ParseInput(string filePath)
        {
            var charMap = new Dictionary<char, Tile>
            {
                { '|', Tile.VerticalPipe },
                { '-', Tile.HorizontalPipe },
                { 'L', Tile.NorthEastBend },
                { 'J', Tile.NorthWestBend },
                { 'F', Tile.SouthEastBend },
                { '7', Tile.SouthWestBend },
                { '.', Tile.Ground },
                { 'S', Tile.Start }
            };

            return
                File.ReadAllLines(filePath).
                Select(line =>
                    line.
                    Select(lineChar => charMap[lineChar]).
                    ToList()).
                ToList();
        }

        public static int Part1(List<List<Tile>> input)
        {
            Func<(int, int), bool> inBounds = (point) =>
            {
                (int pointY, int pointX) = point;
                if (pointY < 0 || input.Count <= pointY)
                {
                    return false;
                }
                else if (pointX < 0 || input[pointY].Count <= pointX)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            };

            Func<Tile, (int, int), (int, int), List<(int,int)>> getNextPoints = (tileType, prevPoint, currPoint) =>
            {
                (int currY, int currX) = currPoint;
                var nextPointsMap = new Dictionary<Tile, List<(int, int)>>
                {
                    { Tile.VerticalPipe, [(currY - 1, currX), (currY + 1, currX)] },
                    { Tile.HorizontalPipe, [(currY, currX - 1), (currY, currX + 1)] },
                    { Tile.NorthEastBend, [(currY - 1, currX), (currY, currX + 1)] },
                    { Tile.NorthWestBend, [(currY - 1, currX), (currY, currX - 1)] },
                    { Tile.SouthEastBend,  [(currY + 1, currX), (currY, currX + 1)]},
                    { Tile.SouthWestBend,  [(currY + 1, currX), (currY, currX - 1)]},
                    { Tile.Start, [(currY + 1, currX), (currY - 1, currX), (currY, currX + 1), (currY, currX - 1)] }
                };
                return
                    (from nextPoint in nextPointsMap[tileType]
                    where nextPoint != prevPoint
                    select nextPoint).ToList();
            };

            int startingY =
                input.FindIndex(0, tileRow => tileRow.Contains(Tile.Start));
            int startingX =
                input[startingY].
                FindIndex(0, tileCol => tileCol == Tile.Start);
            var startingPoint = (startingY, startingX);
            var startingPrevPoint = (int.MinValue, int.MinValue);

            var traversalQueue = new Queue<((int, int), (int, int), int)>();
            var startingQueueTuple = (startingPrevPoint, (startingY, startingX), 0);
            traversalQueue.Enqueue(startingQueueTuple);
            var visitedSet = new HashSet<(int, int)>();
            while (traversalQueue.Count != 0)
            {
                ((int, int) prevPoint, (int, int) currPoint, int currSteps) = traversalQueue.Dequeue();

                if (!inBounds(currPoint))
                {
                    continue;
                }
                (int currY, int currX) = currPoint;
                if (input[currY][currX] == Tile.Ground)
                {
                    continue;
                }

                if (visitedSet.Contains(currPoint))
                {
                    return currSteps;
                }

                visitedSet.Add(currPoint);
                foreach (var toEnqueue in getNextPoints(input[currY][currX], prevPoint, currPoint))
                {
                    var newQueueEntry = (currPoint, toEnqueue, currSteps + 1);
                    traversalQueue.Enqueue(newQueueEntry);
                }
            }

            throw new Exception("STuff");
        }

        static void Main(string[] args)
        {
            var parsedInput = Program.ParseInput("Input.txt");
            var part1Result = Part1(parsedInput);
            Console.WriteLine($"Part 1 Result: {part1Result}");
        }
    }
}
