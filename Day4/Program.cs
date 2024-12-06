// Input
string[] lines = File.ReadAllLines($"{Directory.GetCurrentDirectory()}/input.txt");
char[][] map = lines.Select(line => line.ToCharArray()).ToArray();

// Part 1
var results = FindXMASInAllDirections(lines);

Console.WriteLine(results.Count);

static List<(int row, int col, string direction)> FindXMASInAllDirections(string[] map)
{
    var results = new List<(int row, int col, string direction)>();
    int rows = map.Length;
    int cols = map[0].Length;

    // Define all 8 directions
    (int dr, int dc, string dir)[] directions = {
        (-1, 0, "Up"),
        (1, 0, "Down"),
        (0, 1, "Right"),
        (0, -1, "Left"),
        (-1, 1, "Up-Right"),
        (-1, -1, "Up-Left"),
        (1, 1, "Down-Right"),
        (1, -1, "Down-Left")
    };

    // Check each position in the map
    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            // Try each direction from current position
            foreach (var (dr, dc, dir) in directions)
            {
                if (CheckXMAS(map, row, col, dr, dc))
                {
                    results.Add((row, col, dir));
                }
            }
        }
    }

    return results;
}

