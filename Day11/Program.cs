// Input
string input = File.ReadAllText("input.txt");
List<long> stones = input.Split(' ')
    .Select(long.Parse)
    .ToList();

// Part 1
for (int i = 0; i < 25; i++)
{
    for (int j = 0; j < stones.Count; j++)
    {
        if (stones[j] == 0)
        {
            stones[j] = 1;
        }
        else if (Math.Abs(stones[j]).ToString().Length % 2 == 0)
        {
            SplitStones(stones, j);
            ++j; // Skip the newly added stone
        }
        else
        {
            stones[j] *= 2024;
        }
    }
}

Console.WriteLine(stones.Count);

void SplitStones(List<long> stones, int index)
{
    // Convert to string to get the length
    string numStr = Math.Abs(stones[index]).ToString();
    int length = numStr.Length;
    int halfLength = length / 2;
    
    // Calculate the divisor (10 raised to power of half length) to split the numbers
    long divisor = (int)Math.Pow(10, halfLength);
    long firstHalf = stones[index] / divisor;
    long secondHalf = stones[index] % divisor;

    // Construct new stones
    stones[index] = firstHalf;
    stones.Insert(index + 1, secondHalf);
}