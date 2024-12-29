string[] input = File.ReadAllLines("input.txt");
int rows = input.Length;
int cols = input[0].Length;
int[,] grid = new int[rows, cols];
(int, int) start = (-1, -1);
(int, int) end = (-1, -1);

// Initialize the grid and find the start (S) and end (E) positions
for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < cols; j++)
    {
        if (input[i][j] == '#')
        {
            grid[i, j] = -1; // Wall
        }
        else
        {
            grid[i, j] = int.MaxValue; // Unvisited
            if (input[i][j] == 'S')
            {
                start = (i, j);
                grid[i, j] = 0; // Start
            }
            else if (input[i][j] == 'E')
            {
                end = (i, j);
            }
        }
    }
}

// Walk the path from S to E
WalkPath(grid, start, end);

// Find and list all valid cheating routes
List<(int, int, int)> cheats = FindCheatingRoutes(grid);
cheats.Sort((a, b) => a.Item3.CompareTo(b.Item3)); // Sort by steps saved

Console.WriteLine("Cheats in order of steps saved:");
foreach (var cheat in cheats)
{
    Console.WriteLine($"Cheat at ({cheat.Item1}, {cheat.Item2}) saves {cheat.Item3} steps.");
}
Console.WriteLine($"Total cheats: {cheats.Count}");

void WalkPath(int[,] grid, (int, int) start, (int, int) end)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    var directions = new (int, int)[]
    {
        (-1, 0), // Up
        (1, 0),  // Down
        (0, -1), // Left
        (0, 1)   // Right
    };

    var queue = new Queue<(int, int)>();
    queue.Enqueue(start);

    while (queue.Count > 0)
    {
        var (x, y) = queue.Dequeue();

        foreach (var (dx, dy) in directions)
        {
            int newX = x + dx;
            int newY = y + dy;

            if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && grid[newX, newY] != -1 && grid[newX, newY] == int.MaxValue)
            {
                grid[newX, newY] = grid[x, y] + 1;
                queue.Enqueue((newX, newY));
            }
        }
    }
}

List<(int, int, int)> FindCheatingRoutes(int[,] grid)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    var cheats = new List<(int, int, int)>();

    for (int i = 1; i < rows - 1; i++)
    {
        for (int j = 1; j < cols - 1; j++)
        {
            if (grid[i, j] == -1) // Wall
            {
                // Check vertical and horizontal neighbors
                if ((grid[i - 1, j] != -1 && grid[i + 1, j] != -1) || (grid[i, j - 1] != -1 && grid[i, j + 1] != -1))
                {
                    int minSteps = int.MaxValue;
                    int maxSteps = int.MinValue;

                    // Check vertical neighbors
                    if (grid[i - 1, j] != -1 && grid[i + 1, j] != -1)
                    {
                        minSteps = Math.Min(grid[i - 1, j], grid[i + 1, j]);
                        maxSteps = Math.Max(grid[i - 1, j], grid[i + 1, j]);
                    }

                    // Check horizontal neighbors
                    if (grid[i, j - 1] != -1 && grid[i, j + 1] != -1)
                    {
                        minSteps = Math.Min(minSteps, Math.Min(grid[i, j - 1], grid[i, j + 1]));
                        maxSteps = Math.Max(maxSteps, Math.Max(grid[i, j - 1], grid[i, j + 1]));
                    }

                    // Calculate time saved by cheating
                    int timeSaved = maxSteps - minSteps - 2;
                    if (timeSaved >= 100)
                    {
                        cheats.Add((i, j, timeSaved));
                    }
                }
            }
        }
    }

    return cheats;
}