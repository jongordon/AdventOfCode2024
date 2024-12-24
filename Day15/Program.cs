// Input
string input = File.ReadAllText("input.txt").Replace("\r\n", "\n");
var sections = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
var mapStr = sections[0].Split('\n');
var moves = sections[1];

int rows = mapStr.Length;
int cols = mapStr[0].Length;
char[,] grid = new char[rows, cols];
int currentRow = -1, currentCol = -1;

// Initialise grid and locate start pos
for (int r = 0; r < rows; r++)
{
    for (int c = 0; c < cols; c++)
    {
        grid[r, c] = mapStr[r][c];
        if (grid[r, c] == '@')
        {
            currentRow = r;
            currentCol = c;
        }
    }
}

// Part 1
PrintMap();
foreach(var dir in moves)
{
    MoveRobot(dir);
}
PrintMap();
Console.WriteLine(CountObjectPositions());

// Part 2
// Expand grid to double width
grid = new char[rows, cols * 2];
char leftChar, rightChar;
for (int r = 0; r < rows; r++)
{
    for (int c = 0, newC = 0; c < cols; c++, newC += 2)
    {
        switch (mapStr[r][c])
        {
            case '@':
                currentRow = r;
                currentCol = newC;
                leftChar  = '@';
                rightChar = '.';
            break;
            case '#':
                leftChar  = '#';
                rightChar = '#';
            break;
            case 'O':
                leftChar  = '[';
                rightChar = ']';
            break;
            default:
                leftChar  = '.';
                rightChar = '.';
            break;
        }
        grid[r, newC    ] = leftChar;
        grid[r, newC + 1] = rightChar;
    }
}
cols *= 2;
PrintMap();
foreach(var dir in moves)
{
    Console.WriteLine($"Move: {dir}");
    MoveRobot(dir);
    PrintMap();
}
Console.WriteLine(CountObjectPositions());

void MoveRobot(char direction)
{
    // Direction offsets
    int dY = 0, dX = 0;
    switch (direction)
    {
        case '^': dY = -1; break; // Up
        case 'v': dY = 1; break;  // Down
        case '<': dX = -1; break; // Left
        case '>': dX = 1; break;  // Right
        default: return; // Ignore invalid directions
    }

    int newRow = currentRow + dY;
    int newCol = currentCol + dX;

    if (grid[newRow, newCol] == '.') // Open space
    {
        UpdateRobot(newRow, newCol);
    }
    else if (grid[newRow, newCol] == 'O') // Object to push
    {
        int pushRow = newRow;
        int pushCol = newCol;

        // Continue pushing boxes until hitting an empty space or obstacle
        while (grid[pushRow + dY, pushCol + dX] == 'O')
        {
            pushRow += dY;
            pushCol += dX;
        }

        // If the final space is empty, move the boxes
        if (grid[pushRow + dY, pushCol + dX] == '.')
        {
            grid[pushRow + dY, pushCol + dX] = 'O';

            // Move all intermediate boxes one step
            while (pushRow != newRow || pushCol != newCol)
            {
                grid[pushRow, pushCol] = 'O';
                grid[pushRow - dY, pushCol - dX] = '.';
                pushRow -= dY;
                pushCol -= dX;
            }

            // Move the robot
            UpdateRobot(newRow, newCol);
        }
    }
    else if (grid[newRow, newCol] == '[' || grid[newRow, newCol] == ']')
    {
        if (TryMoveBoxes(newRow, newCol, dY, dX))
        {
            UpdateRobot(newRow, newCol); // Move the robot
        }
    }
    // Obstacles and invalid moves are ignored
}

void UpdateRobot(int newRow, int newCol)
{
    grid[currentRow, currentCol] = '.';
    grid[newRow, newCol] = '@';
    currentRow = newRow;
    currentCol = newCol;
}

long CountObjectPositions()
{
    long count = 0;
    for (int r = 0; r < rows; r++)
    {
        for (int c = 0; c < cols; c++)
        {
            if (grid[r, c] == 'O' || grid[r, c] == '[')
            {
                count += (100 * r) + c;
            }
        }
    }
    return count;
}

void PrintMap()
{
    for (int r = 0; r < rows; r++)
    {
        for (int c = 0; c < cols; c++)
        {
            Console.Write(grid[r, c]);
        }
        Console.WriteLine();
    }
}

bool TryMoveBoxes(int startRow, int startCol, int dY, int dX)
{
    // Collect the positions of all the boxes in this direction
    List<(int, int)> boxesToMove = new List<(int, int)>();
    SearchBoxes(startRow, startCol, dY, dX, boxesToMove);

    // Check if we can move all parts in the desired direction
    foreach (var part in boxesToMove)
    {
        int newRow = part.Item1 + dY;
        int newCol = part.Item2 + dX;

        // Check if the new position is valid (not into an obstacle or off the grid)
        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || (grid[newRow, newCol] != '.' && !boxesToMove.Contains((newRow, newCol))))
        {
            return false;
        }
    }

    // Remove all ] entries from the list by scanning list backwards
    for (int i = boxesToMove.Count - 1; i >= 0; i--)
    {
        if (grid[boxesToMove[i].Item1, boxesToMove[i].Item2] == ']')
        {
            grid[boxesToMove[i].Item1, boxesToMove[i].Item2] = '.';
            boxesToMove.RemoveAt(i);
        }
    }

    // Move all parts - after reversing the list because we did a BFS
    boxesToMove.Reverse();
    foreach (var part in boxesToMove)
    {
        // Move [ part of box
        grid[part.Item1, part.Item2] = '.';
        grid[part.Item1 + dY, part.Item2 + dX] = '[';
        // Add ] part of box
        grid[part.Item1 + dY, part.Item2 + dX + 1] = ']';
    }

    return true;
}

void SearchBoxes(int row, int col, int dY, int dX, List<(int, int)> boxesToMove)
{
    // Create a queue for BFS
    Queue<(int, int)> queue = new Queue<(int, int)>();
    // Create a set to keep track of visited nodes
    HashSet<(int, int)> visited = new HashSet<(int, int)>();

    // Enqueue the starting position and mark it as visited
    queue.Enqueue((row, col));
    visited.Add((row, col));

    // Continue until all boxes in the chain are found
    while (queue.Count > 0)
    {
        // Dequeue the first box (entry) in the queue
        var (r, c) = queue.Dequeue();
        // Find other half of box and add to queue to search next (if not already visited)
        if (grid[r, c] == '[' && !visited.Contains((r, c + 1)))
        {
            queue.Enqueue((r, c + 1));
            visited.Add((r, c + 1));
        }
        else if (grid[r, c] == ']' && !visited.Contains((r, c - 1)))
        {
            queue.Enqueue((r, c - 1));
            visited.Add((r, c - 1));
        }

        // Now check next position
        if (grid[r + dY, c + dX] == '[' || grid[r + dY, c + dX] == ']')
        {
            if (!visited.Contains((r + dY, c + dX)))
            {
                queue.Enqueue((r + dY, c + dX));
                visited.Add((r + dY, c + dX));
            }
        }
    }
    boxesToMove.AddRange(visited);
}