// Input
using System.Runtime.CompilerServices;

string input = File.ReadAllText("input.txt");
var sections = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
var mapStr = sections[0].Split('\n').ToArray();
int rows = mapStr.Length;
int cols = mapStr[0].Length;
char[,] grid = new char[rows, cols];
for (int r = 0; r < rows; r++)
{
    for (int c = 0; c < cols; c++)
    {
        grid[r, c] = mapStr[r][c];
    }
}
var moves = sections[1];

// Part 1
PrintMap();
foreach(var dir in moves)
{
    MoveRobot(dir);
}
PrintMap();
System.Console.WriteLine(CountObjectPositions());

void MoveRobot(char direction)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    int currentRow = -1, currentCol = -1;

    // Find player position
    for (int r = 0; r < rows; r++)
    {
        for (int c = 0; c < cols; c++)
        {
            if (grid[r, c] == '@')
            {
                currentRow = r;
                currentCol = c;
                break;
            }
        }
        if (currentRow != -1) break;
    }

    // Direction offsets
    int dY = 0, dX = 0;
    switch (direction)
    {
        case '^': dY = -1; break;
        case 'v': dY = 1; break;
        case '<': dX = -1; break;
        case '>': dX = 1; break;
    }

    int newRow = currentRow + dY;
    int newCol = currentCol + dX;

    // Validate move
    if (grid[newRow, newCol] == '.') // Open space
    {
        grid[currentRow, currentCol] = '.';
        grid[newRow, newCol] = '@';
    }
    else if (grid[newRow, newCol] == 'O') // Object to push
    {
        int pushRow = newRow + dY;
        int pushCol = newCol + dX;

        // Try pushing the object(s) to the next empty space
        while (grid[pushRow, pushCol] == 'O') // Multiple objects in a line
        {
            int nextRow = pushRow + dY;
            int nextCol = pushCol + dX;

            // If we find an empty space, move the object
            if (grid[nextRow, nextCol] == '.')
            {
                grid[nextRow, nextCol] = 'O';
                grid[pushRow, pushCol] = '.';

                // Move the robot only after the last object is pushed
                grid[currentRow, currentCol] = '.';
                grid[newRow, newCol] = '@';
                break; // Exit the loop after moving the robot
            }

            pushRow = nextRow;
            pushCol = nextCol;
        }

        // If only one object was pushed, still move the robot
        if (grid[pushRow, pushCol] == '.')
        {
            grid[pushRow, pushCol] = 'O';
            grid[currentRow, currentCol] = '.';
            grid[newRow, newCol] = '@';
        }
    }
    else
    {
        // Obstacle, can't move
    }
}

long CountObjectPositions()
{
    long count = 0;
    for (int r = 0; r < grid.GetLength(0); r++)
    {
        for (int c = 0; c < grid.GetLength(1); c++)
        {
            if (grid[r, c] == 'O')
            {
                count += (100 * r) + c;
            }
        }
    }
    return count;
}

void PrintMap()
{
    for (int r = 0; r < grid.GetLength(0); r++)
    {
        for (int c = 0; c < grid.GetLength(1); c++)
        {
            Console.Write(grid[r, c]);
        }
        Console.WriteLine();
    }
}