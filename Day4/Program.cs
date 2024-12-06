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

static bool CheckXMAS(string[] map, int startRow, int startCol, int dr, int dc)
{
    int rows = map.Length;
    int cols = map[0].Length;
    string target = "XMAS";

    // Check if we can fit "XMAS" in this direction
    for (int i = 0; i < target.Length; i++)
    {
        int newRow = startRow + (i * dr);
        int newCol = startCol + (i * dc);

        // Check bounds
        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
            return false;

        // Check character match
        if (map[newRow][newCol] != target[i])
            return false;
    }

    return true;
}

// Part 2
var results2 = FindXShapedMAS(lines);
Console.WriteLine(results2.Count);

static List<(int row, int col)> FindXShapedMAS(string[] map)
{
    var results = new List<(int row, int col)>();
    int rows = map.Length;
    int cols = map[0].Length;

    // Check each position in the map
    for (int row = 1; row < rows - 1; row++)  // Start at 1 and end at rows-1 to ensure space for pattern
    {
        for (int col = 1; col < cols - 1; col++)  // Start at 1 and end at cols-1 to ensure space for pattern
        {
            // Check if current position is 'A'
            if (map[row][col] == 'A')
            {
                // Check all four possible diagonal combinations
                bool topLeftToBottomRight = CheckDiagonal(map, row, col, -1, -1, 1, 1);
                bool topRightToBottomLeft = CheckDiagonal(map, row, col, -1, 1, 1, -1);
                bool bottomLeftToTopRight = CheckDiagonal(map, row, col, 1, -1, -1, 1);
                bool bottomRightToTopLeft = CheckDiagonal(map, row, col, 1, 1, -1, -1);

                if ((topLeftToBottomRight && topRightToBottomLeft) ||
                    (bottomLeftToTopRight && bottomRightToTopLeft) ||
                    (topLeftToBottomRight && bottomLeftToTopRight) ||
                    (topRightToBottomLeft && bottomRightToTopLeft))
                {
                    results.Add((row, col));
                }
            }
        }
    }

    return results;
}

static bool CheckDiagonal(string[] map, int centerRow, int centerCol, 
                         int dr1, int dc1, int dr2, int dc2)
{
    // Check bounds first
    if (centerRow + dr1 < 0 || centerRow + dr1 >= map.Length ||
        centerRow + dr2 < 0 || centerRow + dr2 >= map.Length ||
        centerCol + dc1 < 0 || centerCol + dc1 >= map[0].Length ||
        centerCol + dc2 < 0 || centerCol + dc2 >= map[0].Length)
    {
        return false;
    }

    // Check for 'M' in one direction and 'S' in the other
    return (map[centerRow + dr1][centerCol + dc1] == 'M' &&
            map[centerRow + dr2][centerCol + dc2] == 'S');
}
