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

// Part 2
safe = 0;
foreach (var level in levels)
{
    if (isSafe(level))
    {
        ++safe;
        continue;
    }

    // Currently unsafe, try remove an index one at a time to see if we find a safe one
    for (int i = 0; i < level.Count; i++)
    {
        List<int> levelCopy = new List<int>(level);
        levelCopy.RemoveAt(i);
        if (isSafe(levelCopy))
        {
            ++safe;
            break;
        }
    }
}
System.Console.WriteLine(safe);

bool isSafe(List<int> level)
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
        return false;
    }

    // Can mark as safe
    return true;
}