// Input
List<List<int>> levels = new List<List<int>>();
foreach (string line in File.ReadLines($"{Directory.GetCurrentDirectory()}/input.txt"))
{
    List<int> level = new List<int>();
    string[] report = line.Split(' ');
    foreach (var levelStr in report)
    {
        level.Add(int.Parse(levelStr));
    }
    levels.Add(level);
}

// Part 1
int safe = 0;
foreach (var level in levels)
{
    bool isAscending = true;
    bool isDescending = true;
    bool isWithinRange = true;
    
    for (int i = 1; i < level.Count; i++)
    {
        if (level[i] <= level[i - 1])
            isAscending = false;
        if (level[i] >= level[i - 1])
            isDescending = false;

        int delta = Math.Abs(level[i] - level[i - 1]);
        if (delta < 1 || delta > 3)
        {
            isWithinRange = false;
            break;
        }
    }

    if ((!isAscending && !isDescending) || !isWithinRange) 
    {
        // Unsafe
        continue;
    }

    // Can mark as safe
    ++safe;
}
System.Console.WriteLine(safe);

