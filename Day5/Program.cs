// Input
string[] lines = File.ReadAllLines($"{Directory.GetCurrentDirectory()}/input.txt");

// Split rules and updates
int separatorIndex = Array.IndexOf(lines, string.Empty);
string[] rulesStr = lines[..separatorIndex];
string[] messages = lines[(separatorIndex + 1)..];

// Parse rules
List<(int, int)> rules = rulesStr
    .Select(rule => rule.Split('|'))
    .Select(split => (int.Parse(split[0]), int.Parse(split[1])))
    .ToList();

// Parse updates
List<int[]> updates = messages
    .Select(update => update.Split(',').Select(int.Parse).ToArray())
    .ToList();

// Part 1
List<int> middlePages = new List<int>();
for (int i = updates.Count - 1; i >= 0; i--)    // Iterate backwards to allow safe removal
{
    int[] update = updates[i];
    bool isValidUpdate = true;
    foreach (var (first, second) in rules)
    {
        // Check these pages exist in this update
        int firstPageIndex = Array.IndexOf(update, first);
        int secondPageIndex = Array.IndexOf(update, second);
        if (firstPageIndex != -1 && secondPageIndex != -1)
        {
            // Now check first comes before second, invalid if it does
            if (firstPageIndex >= secondPageIndex)
            {
                isValidUpdate = false;
                break;
            }
        }
    }

    // If remained valid after check, add middle page number to list
    if (isValidUpdate)
    {
        int middleIndex = update.Length / 2;
        middlePages.Add(update[middleIndex]);
        // We don't need this now (helps Part 2)
        updates.RemoveAt(i);
    }
}

System.Console.WriteLine(middlePages.Sum());

// Part 2
middlePages.Clear();
foreach (var update in updates)
{
    bool madeChanges;
    do
    {
        madeChanges = false;
        // Check every rule, and swap the numbers if they are in the wrong order
        foreach (var (first, second) in rules)
        {
            int firstPageIndex = Array.IndexOf(update, first);
            int secondPageIndex = Array.IndexOf(update, second);
            if (firstPageIndex != -1 && secondPageIndex != -1)
            {
                if (firstPageIndex >= secondPageIndex)
                {
                    update[firstPageIndex] = second;
                    update[secondPageIndex] = first;
                    madeChanges = true;
                }
            }
        }
    } while (madeChanges);
            
    int middleIndex = update.Length / 2;
    middlePages.Add(update[middleIndex]);
}

System.Console.WriteLine(middlePages.Sum());
