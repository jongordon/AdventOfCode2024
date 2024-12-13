// Input
string[] input = File.ReadAllLines("input.txt");
var machinesStr = input.Aggregate(new List<List<string>> { new List<string>() },
        (list, line) => {
            if (string.IsNullOrWhiteSpace(line)) list.Add(new List<string>());
            else list[^1].Add(line);
            return list;
        })
    .Where(group => group.Any())
    .ToList();

var machines = new List<(Move a, Move b, Move p)>();

foreach (var machineStr in machinesStr)
{
    var a = ParseMove(machineStr[0], new[] {'+', ','});
    var b = ParseMove(machineStr[1], new[] {'+', ','});
    var p = ParseMove(machineStr[2], new[] {'=', ','});

    machines.Add((a, b, p));
}

// Part 1
int totalCost = 0;

foreach (var machine in machines)
{
    var result = FindMinimumMultiples(
        (machine.a.x, machine.a.y),
        (machine.b.x, machine.b.y),
        (machine.p.x, machine.p.y));

    if (result.HasValue)
    {
        totalCost += (result.Value.count1 * 3) + result.Value.count2;
    }
}
System.Console.WriteLine(totalCost);

Move ParseMove(string str, char[] delimiters) => new Move 
{
    x = int.Parse(str.Split(delimiters)[1]),
    y = int.Parse(str.Split(delimiters)[3])
};

(int count1, int count2)? FindMinimumMultiples(
    (int x, int y) input1, 
    (int x, int y) input2, 
    (int x, int y) target)
{
    // Start with maximum possible of input2 and work backwards
    int maxInput2X = target.x / input2.x;
    int maxInput2Y = target.y / input2.y;
    
    // Try each possibility of input2, starting from the maximum
    for (int count2 = Math.Max(maxInput2X, maxInput2Y); count2 >= 0; count2--)
    {
        int remainingX = target.x - (count2 * input2.x);
        int remainingY = target.y - (count2 * input2.y);
        
        // If remainingX is perfectly divisible by input1.x and 
        // the same number of input1 satisfies remainingY
        if (remainingX >= 0 && remainingY >= 0)
        {
            int neededCount1X = remainingX / input1.x;
            int neededCount1Y = remainingY / input1.y;
            
            if (neededCount1X == neededCount1Y && 
                remainingX % input1.x == 0 && 
                remainingY % input1.y == 0)
            {
                return (neededCount1X, count2);
            }
        }
    }
    
    return null; // No exact solution found
}

struct Move
{
    public int x;
    public int y;
}
