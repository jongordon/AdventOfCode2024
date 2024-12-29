string[] input = File.ReadAllLines("input.txt");
string[] towels = input[0].Split(", ");
List<string> designs = new List<string>();
for (int i = 2; i < input.Length; i++)
{
    designs.Add(input[i]);
}

// Part 1
int validDesigns = 0;

foreach (var design in designs)
{
    if (CanMakeDesign(design, towels))
    {
        validDesigns++;
    }
}

Console.WriteLine(validDesigns);

bool CanMakeDesign(string design, string[] towels)
{
    // Base condition to exit recursion
    if (string.IsNullOrEmpty(design))
    {
        return true;
    }

    foreach (var towel in towels)
    {
        if (design.StartsWith(towel))
        {
            if (CanMakeDesign(design.Substring(towel.Length), towels))
            {
                return true;
            }
        }
    }

    return false;
}

// Part 2
long totalWays = 0;
HashSet<string> hashedTowels = new HashSet<string>(input[0].Split(", "));
var memo = new Dictionary<string, long>();

foreach (var design in designs)
{
    totalWays += CountWaysToMakeDesign(design, hashedTowels, memo);
}

Console.WriteLine(totalWays);

long CountWaysToMakeDesign(string design, HashSet<string> towels, Dictionary<string, long> memo)
{
    // Base condition to exit recursion
    if (string.IsNullOrEmpty(design))
    {
        return 1;
    }

    // Check if the result is already computed for the current substring
    if (memo.ContainsKey(design))
    {
        return memo[design];
    }

    long count = 0;
    foreach (var towel in towels)
    {
        if (design.StartsWith(towel))
        {
            count += CountWaysToMakeDesign(design.Substring(towel.Length), towels, memo);
        }
    }

    // Store the result in the memo dictionary for the current substring
    memo[design] = count;

    // Debugging output
    Console.WriteLine($"Memoizing: {design} -> {count}");

    return count;
}