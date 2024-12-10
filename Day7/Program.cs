// Input
using System.Numerics;

string[] lines = File.ReadAllLines($"{Directory.GetCurrentDirectory()}/input.txt");

// Part 1
Solve(lines, includeConcatenation: false);

static bool CanBeEqual(long testValue, List<int> numbers, bool includeConcatenation)
{
    // We use a recursive approach to evaluate all possible combinations of operators.
    return CheckCombinations(testValue, numbers, numbers[0], 1, includeConcatenation);
}

// Helper function to recursively check all combinations
static bool CheckCombinations(long testValue, List<int> numbers, long currentResult, int index, bool includeConcatenation)
{
    // If we've processed all numbers, check if the result matches the test value
    if (index == numbers.Count)
    {
        return currentResult == testValue;
    }

    // Get the next number to process
    int nextNumber = numbers[index];

    // Always try adding and multiplying
    bool result = CheckCombinations(testValue, numbers, currentResult + nextNumber, index + 1, includeConcatenation) ||
                 CheckCombinations(testValue, numbers, currentResult * nextNumber, index + 1, includeConcatenation);

    // Only try concatenation if it's enabled
    if (includeConcatenation)
    {
        result = result || CheckCombinations(testValue, numbers, ConcatenateNumbers(currentResult, nextNumber), index + 1, includeConcatenation);
    }

    return result;
}

// Main function to process each equation and keep track of the running total of test values
static void Solve(string[] inputLines, bool includeConcatenation)
{
    long runningTotal = 0;  // Initialize the running total

    foreach (var line in inputLines)
    {
        var parts = line.Split(':');
        long testValue = long.Parse(parts[0].Trim());
        List<int> numbers = parts[1].Trim().Split(' ').Select(int.Parse).ToList();

        // Check if the current test case can be made valid
        if (CanBeEqual(testValue, numbers, includeConcatenation))
        {
            // Add the test value to the running total if it's valid
            runningTotal += testValue;
        }
    }

    // Final running total after all test cases
    Console.WriteLine($"{runningTotal}");
}

// Part 2
Solve(lines, includeConcatenation: true);

// Helper function to concatenate numbers
static long ConcatenateNumbers(long first, long second)
{
    string secondStr = second.ToString();
    return first * (long)Math.Pow(10, secondStr.Length) + second;
}
