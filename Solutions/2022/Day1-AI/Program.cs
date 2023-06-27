using System;
using System.IO;
using System.Linq;

/*
 * Original Solution
 * 
string filePath = "Input.txt";  // replace with actual file path

// Read the contents of the file
string input = File.ReadAllText(filePath);

// Split the input into individual elves' inventories
string[] elfInventories = input.Split(new[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

// Create a list to hold the total calories for each elf
int[] totalCaloriesPerElf = new int[elfInventories.Length];

// Loop through each elf's inventory
for (int i = 0; i < elfInventories.Length; i++)
{
    // Split the elf's inventory into individual food items and convert to integers
    int[] foodItems = elfInventories[i].Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

    // Calculate the total calories for this elf
    totalCaloriesPerElf[i] = foodItems.Sum();
}

// Sort the array in descending order
var sortedCaloriesPerElf = totalCaloriesPerElf.OrderByDescending(x => x).ToArray();

// The first element is the maximum
int maxCalories = sortedCaloriesPerElf[0];
Console.WriteLine($"The elf carrying the most calories is carrying {maxCalories} calories.");

// Sum the top three values
int sumOfTopThreeCalories = sortedCaloriesPerElf.Take(3).Sum();
Console.WriteLine($"The total calories carried by the top three elves is {sumOfTopThreeCalories} calories.");
*/

using System;
using System.IO;
using System.Linq;

var filePath = "Input.txt";
var totalCaloriesPerElf = File.ReadAllText(filePath)
                             .Split(new[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(elfInventory => elfInventory.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                                                 .Select(int.Parse)
                                                                 .Sum())
                             .OrderByDescending(x => x)
                             .ToArray();

Console.WriteLine($"The elf carrying the most calories is carrying {totalCaloriesPerElf.First()} calories.");
Console.WriteLine($"The total calories carried by the top three elves is {totalCaloriesPerElf.Take(3).Sum()} calories.");