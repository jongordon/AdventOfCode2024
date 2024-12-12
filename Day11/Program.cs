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

// Part 2
Dictionary<long, long> stonesFast = [];
long total = 0;
input.Split(' ').Select(long.Parse).ToList()
    .ForEach(x => Process(x, 1, ref total));

void Process(long stoneNum, long toAdd, ref long total)
{
    stonesFast.TryGetValue(stoneNum, out long amt);
    stonesFast[stoneNum] = amt + toAdd;
    total += toAdd;
}

for (int blink = 0; blink < 75; blink++)
{
    total = 0;
    var list = stonesFast.ToList();
    stonesFast.Clear();
    foreach (var stone in list)
        if (stone.Key == 0) Process(1, stone.Value, ref total);
        else if (Math.Floor(Math.Log10(stone.Key) + 1) % 2 == 0)
        {   // If digits are an even number, split the number
            long tens = (long)Math.Pow(10, 
                (int)Math.Floor(Math.Log10(stone.Key) + 1) / 2);
            var left = stone.Key / tens;
            var right = stone.Key % tens;
            Process(left, stone.Value, ref total);
            Process(right, stone.Value, ref total);
        }
        else Process(stone.Key * 2024, stone.Value, ref total);
}
Console.WriteLine(total);