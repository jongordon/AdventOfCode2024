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