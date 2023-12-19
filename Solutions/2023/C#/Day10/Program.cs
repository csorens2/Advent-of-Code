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
            Func<(int, int), (int, int), double> distance = (source, dest) =>
            {
                (int sourceY, int sourceX) = source;
                (int destY, int destX) = dest;
                return 
                    Math.Sqrt(
                        Math.Pow(destY - sourceY, 2) +
                        Math.Pow(destX - sourceX, 2));
            };

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

            // DFS to find the loop, and the point with the longest distance
            // Prev-Point, Curr-Point, Visited-Points
            var traversalStack = new Stack<((int, int), (int,int), HashSet<(int, int)>)>();
            var startingPrevPoint = (int.MinValue, int.MinValue);
            var startingStackTuple = (startingPrevPoint, startingPoint, new HashSet<(int, int)>());
            traversalStack.Push(startingStackTuple);
            (int, int)? maxDistancePoint = null;
            while (traversalStack.Count != 0)
            {
                ((int, int) prevPoint, (int, int) currPoint, HashSet<(int, int)> visitedSet) =
                    traversalStack.Pop();

                (int currY, int currX) = currPoint;
                if (!inBounds(currPoint))
                {
                    continue;
                }
                if (input[currY][currX] == Tile.Ground)
                {
                    continue;
                }
                
                if (input[currY][currX] == Tile.Start && startingPrevPoint != prevPoint)
                {
                    var test =
                        visitedSet.
                        Select(visitedPoint => (visitedPoint, distance(startingPoint, visitedPoint))).
                        OrderByDescending(x => x.Item2).ToList();

                    maxDistancePoint =
                        visitedSet.
                        OrderByDescending(visitedPoint => distance(startingPoint, visitedPoint)).
                        First();
                    break;
                }

                if (visitedSet.Contains(currPoint))
                {
                    continue;
                }
                visitedSet.Add(currPoint);

                foreach (var toPush in getNextPoints(input[currY][currX], prevPoint, currPoint))
                {
                    var newStackEntry = (currPoint, toPush, visitedSet);
                    traversalStack.Push(newStackEntry);
                }
            }
            if (maxDistancePoint == null)
            {
                throw new Exception("No loop found");
            }

            var traversalQueue = new Queue<((int, int), (int, int), int, HashSet<(int, int)>)>();
            var startingQueueTuple = (startingPoint, (startingY, startingX), 0, new HashSet<(int, int)>());
            traversalQueue.Enqueue(startingQueueTuple);
            while (traversalQueue.Count != 0)
            {
                ((int, int) prevPoint, (int, int) currPoint, int currSteps, HashSet<(int, int)> visitedSet) =
                    traversalQueue.Dequeue();

                if (currPoint == maxDistancePoint)
                {
                    return currSteps;
                }

                (int currY, int currX) = currPoint;
                if (!inBounds(currPoint))
                {
                    continue;
                }
                if (input[currY][currX] == Tile.Ground)
                {
                    continue;
                }

                if (visitedSet.Contains(currPoint))
                {
                    continue;
                }
                visitedSet.Add(currPoint);

                foreach (var toEnqueue in getNextPoints(input[currY][currX], prevPoint, currPoint))
                {
                    var newQueueEntry = (currPoint, toEnqueue, currSteps + 1, visitedSet);
                    traversalQueue.Enqueue(newQueueEntry);
                }
            }

            throw new Exception("Somehow didn't find point again.");
        }

        static void Main(string[] args)
        {
            var test = Program.ParseInput("Input.txt");
            var test2 = Part1(test);

            Console.WriteLine("Hello, World!");
        }
    }
}
