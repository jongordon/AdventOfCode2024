// Input
string[] lines = File.ReadAllLines($"{Directory.GetCurrentDirectory()}/input.txt");
char[][] map = lines.Select(line => line.ToCharArray()).ToArray();

// Part 1
// Set to store visited positions
HashSet<(int row, int col)> visited = new HashSet<(int row, int col)>();

// Find starting position and direction
int startRow = -1, startCol = -1;
char startDir = ' ';

for (int i = 0; i < map.Length; i++)
{
    for (int j = 0; j < map[i].Length; j++)
    {
        if (map[i][j] is '^' or 'v' or '<' or '>')
        {
            startRow = i;
            startCol = j;
            startDir = map[i][j];
            break;
        }
    }
    if (startRow != -1) break;
}

// Direction vectors (up, right, down, left)
(int dr, int dc)[] directions = { (-1, 0), (0, 1), (1, 0), (0, -1) };

// Current position and direction index
int currentRow = startRow;
int currentCol = startCol;
int dirIndex = startDir switch
{
    '^' => 0,
    '>' => 1,
    'v' => 2,
    '<' => 3,
    _ => throw new Exception("Invalid starting direction")
};

// Add starting position to visited set
visited.Add((currentRow, currentCol));

while (true)
{
    // Try to move in current direction
    int nextRow = currentRow + directions[dirIndex].dr;
    int nextCol = currentCol + directions[dirIndex].dc;

    // Check if we're out of bounds
    if (nextRow < 0 || nextRow >= map.Length || 
        nextCol < 0 || nextCol >= map[0].Length)
    {
        break; // We've left the map
    }

    // Check if we hit an obstacle
    if (map[nextRow][nextCol] == '#')
    {
        // Turn right (increment direction index)
        dirIndex = (dirIndex + 1) % 4;
        continue;
    }

    // Move to next position
    currentRow = nextRow;
    currentCol = nextCol;
    visited.Add((currentRow, currentCol));
}

// Output result
Console.WriteLine($"{visited.Count}");
