// Input
using System.Numerics;

string[] lines = File.ReadAllLines($"{Directory.GetCurrentDirectory()}/input.txt");

// Part 1
Solve(lines);

static bool CanBeEqual(long testValue, List<int> numbers)
{
    // We use a recursive approach to evaluate all possible combinations of operators.
    return CheckCombinations(testValue, numbers, numbers[0], 1);
}

// Helper function to recursively check all combinations
static bool CheckCombinations(long testValue, List<int> numbers, long currentResult, int index)
{
    // If we've processed all numbers, check if the result matches the test value
    if (index == numbers.Count)
    {
        return currentResult == testValue;
    }

    // Get the next number to process
    int nextNumber = numbers[index];

    // Try adding and multiplying the next number
    return CheckCombinations(testValue, numbers, currentResult + nextNumber, index + 1) ||
            CheckCombinations(testValue, numbers, currentResult * nextNumber, index + 1);
}

// Main function to process each equation and keep track of the running total of test values
static void Solve(string[] inputLines)
{
    long runningTotal = 0;  // Initialize the running total

    foreach (var line in inputLines)
    {
        var parts = line.Split(':');
        long testValue = long.Parse(parts[0].Trim());
        List<int> numbers = parts[1].Trim().Split(' ').Select(int.Parse).ToList();

        // Check if the current test case can be made valid
        if (CanBeEqual(testValue, numbers))
        {
            // Add the test value to the running total if it's valid
            runningTotal += testValue;
        }
    }

    // Final running total after all test cases
    Console.WriteLine("Final running total: " + runningTotal);
}