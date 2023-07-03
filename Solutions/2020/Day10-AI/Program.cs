using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

/*
 * Part 1
 * 
// Read the adapter ratings from the input file
List<int> adapters = File.ReadLines("Input.txt").Select(int.Parse).ToList();

// Add the outlet joltage and device's built-in adapter joltage to the list
adapters.Add(0);
adapters.Add(adapters.Max() + 3);

// Sort the list in ascending order
adapters.Sort();

// Create counters for the joltage differences
int oneJoltDiffs = 0, threeJoltDiffs = 0;

// Iterate over the list and count the joltage differences
for (int i = 1; i < adapters.Count; i++)
{
    int diff = adapters[i] - adapters[i - 1];

    if (diff == 1)
        oneJoltDiffs++;
    else if (diff == 3)
        threeJoltDiffs++;
}

// Print the product of the number of 1-jolt differences and the number of 3-jolt differences
Console.WriteLine(oneJoltDiffs * threeJoltDiffs);
*/

/*
 * Part 2
 * 
var adapters = new List<int> { 0 };  // Add the outlet
adapters.AddRange(File.ReadAllLines("Input.txt").Select(int.Parse));
adapters.Sort();
adapters.Add(adapters.Last() + 3);  // Add your device

var paths = new Dictionary<int, long> { { 0, 1 } }; // The outlet can be reached in one way

foreach (var adapter in adapters.Skip(1))
{
    paths[adapter] = Enumerable.Range(1, 3)
                               .Select(i => adapter - i)
                               .Where(paths.ContainsKey)
                               .Sum(previousAdapter => paths[previousAdapter]);
}

Console.WriteLine(paths[adapters.Last()]);
*/

/*
 * Part 2 Linq
 */

List<int> adapters = File.ReadLines("Input.txt").Select(int.Parse).ToList();
adapters.Add(0);
adapters.Sort();
adapters.Add(adapters.Last() + 3);

Dictionary<int, long> paths = new() { [0] = 1 };

adapters.Aggregate((a, b) =>
{
    if (!paths.ContainsKey(b))
        paths[b] = 0;

    if (paths.ContainsKey(b - 1))
        paths[b] += paths[b - 1];

    if (paths.ContainsKey(b - 2))
        paths[b] += paths[b - 2];

    if (paths.ContainsKey(b - 3))
        paths[b] += paths[b - 3];

    return b;
});

Console.WriteLine(paths[adapters.Last()]);