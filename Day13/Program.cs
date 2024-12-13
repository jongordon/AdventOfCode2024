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
long totalCost = 0;

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

// Part 2
totalCost = 0;

// Adjust all p
foreach (var machine in machines)
{
    var result = FindMinimumMultiples(
        (machine.a.x, machine.a.y),
        (machine.b.x, machine.b.y),
        (machine.p.x + 10000000000000, machine.p.y + 10000000000000));

    if (result.HasValue)
    {
        totalCost += (result.Value.count1 * 3) + result.Value.count2;
    }
}
System.Console.WriteLine(totalCost);

Move ParseMove(string str, char[] delimiters) => new Move 
{
    x = long.Parse(str.Split(delimiters)[1]),
    y = long.Parse(str.Split(delimiters)[3])
};

(long count1, long count2)? FindMinimumMultiples(
    (long x, long y) input1, 
    (long x, long y) input2, 
    (long x, long y) target)
{
    // Using determinant method to solve the system
    decimal determinant = (input1.x * input2.y) - (input2.x * input1.y);
    if (determinant == 0) return null;  // No unique solution
    
    // Using decimal for better precision with large numbers
    decimal c1 = ((target.x * input2.y) - (target.y * input2.x)) / determinant;
    decimal c2 = ((input1.x * target.y) - (input1.y * target.x)) / determinant;
    
    // Verify the solution is valid (integer and non-negative)
    if (c1 != Math.Floor(c1) || c2 != Math.Floor(c2)) return null;
    if (c1 < 0 || c2 < 0) return null;
    
    // Verify the solution actually works
    long count1 = (long)c1;
    long count2 = (long)c2;
    
    // Double check our solution
    if ((count1 * input1.x + count2 * input2.x) != target.x) return null;
    if ((count1 * input1.y + count2 * input2.y) != target.y) return null;
    
    return (count1, count2);
}

struct Move
{
    public long x;
    public long y;
}
