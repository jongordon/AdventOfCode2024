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

// Part 1
int validCheats = FindCheatingRoutes(grid, 2);
Console.WriteLine(validCheats);

// Part 2
validCheats = FindCheatingRoutes(grid, 20);
Console.WriteLine(validCheats);

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

int FindCheatingRoutes(int[,] grid, int maxCheatSteps)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    int validCheats = 0;

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            if (grid[i, j] != -1 && grid[i, j] != int.MaxValue) // Track tile
            {
                for (int x = 0; x < rows; x++)
                {
                    for (int y = 0; y < cols; y++)
                    {
                        if (grid[x, y] != -1 && grid[x, y] != int.MaxValue && (i != x || j != y)) // Another track tile
                        {
                            int manhattanDistance = Math.Abs(i - x) + Math.Abs(j - y);
                            if (manhattanDistance <= maxCheatSteps)
                            {
                                int originalStepsDifference = Math.Abs(grid[i, j] - grid[x, y]);
                                int timeSaved = originalStepsDifference - manhattanDistance;
                                if (timeSaved >= 100)
                                {
                                    validCheats++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Divide by 2 to account for counting each cheat twice
    return validCheats / 2;
}