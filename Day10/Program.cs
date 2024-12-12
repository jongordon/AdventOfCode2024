// Input
string[] input = File.ReadAllLines("input.txt");
int[,] grid = new int[input.Length, input[0].Length];
List<int[]> trailHeads = [];

for (int r = 0; r < input.Length; r++)
    for (int c = 0; c < input[0].Length; c++)
    {
        grid[r,c] = input[r][c] - '0';
        if (grid[r,c] == 0) trailHeads.Add([r,c]);
    }

// Part 1
int validTrailCount = 0;
int rows = grid.GetLength(0);
int cols = grid.GetLength(1);
int[][] directions =
[
    [0, 1],   // right
    [0, -1],  // left
    [1, 0],   // down
    [-1, 0]   // up
];

foreach (var start in trailHeads)
{
    HashSet<(int r, int c)> validTrails = [];   // Track unique valid trails
    var path = new List<(int r, int c)>
    {
        (start[0], start[1])
    };
    FindTrails(start[0], start[1], 1, path, validTrails);   // DFS
    validTrailCount += validTrails.Count;       // Sum each unique trail find for total score
}

void FindTrails(int currentRow, int currentCol, int expectedValue, List<(int r, int c)> currentPath, HashSet<(int r, int c)> validTrails)
{
    if (expectedValue == 10)
    {
        validTrails.Add(currentPath[^1]);
        return;
    }

    foreach (var dir in directions)
    {
        int newRow = currentRow + dir[0];
        int newCol = currentCol + dir[1];

        if (newRow >= 0 && newRow < rows && 
            newCol >= 0 && newCol < cols && 
            grid[newRow, newCol] == expectedValue)
        {
            currentPath.Add((newRow, newCol));
            FindTrails(newRow, newCol, expectedValue + 1, currentPath, validTrails);
            currentPath.RemoveAt(currentPath.Count - 1);
        }
    }
}

Console.WriteLine(validTrailCount);

// Part 2
int totalRating = 0;
foreach (var start in trailHeads)
{
    var path = new List<(int r, int c)>
    {
        (start[0], start[1])
    };
    FindRatings(start[0], start[1], 1, path);   // DFS
}

void FindRatings(int currentRow, int currentCol, int expectedValue, List<(int r, int c)> currentPath)
{
    if (expectedValue == 10)
    {
        ++totalRating;
        return;
    }

    foreach (var dir in directions)
    {
        int newRow = currentRow + dir[0];
        int newCol = currentCol + dir[1];

        if (newRow >= 0 && newRow < rows && 
            newCol >= 0 && newCol < cols && 
            grid[newRow, newCol] == expectedValue)
        {
            currentPath.Add((newRow, newCol));
            FindRatings(newRow, newCol, expectedValue + 1, currentPath);
            currentPath.RemoveAt(currentPath.Count - 1);
        }
    }
}

Console.WriteLine(totalRating);