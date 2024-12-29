// Input
string[] lines = File.ReadAllLines("input.txt");
(int, int)[] bytes = new (int, int)[lines.Length];
foreach (var line in lines)
{
    var parts = line.Split(',');
    bytes[Array.IndexOf(lines, line)] = (int.Parse(parts[0]), int.Parse(parts[1]));
}
int rows = 71;
int cols = 71;
char[,] grid = new char[rows, cols];
for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < cols; j++)
    {
        grid[i, j] = '.';
    }
}
foreach (var b in bytes)
{
    grid[b.Item2, b.Item1] = '#';
    if (Array.IndexOf(bytes, b) == 1024)
    {
        break;
    }
}

// Part 1
Console.WriteLine(FindShortestPath());

int FindShortestPath()
{
    var directions = new (int, int)[]
    {
        (-1, 0), // Up
        (1, 0),  // Down
        (0, -1), // Left
        (0, 1)   // Right
    };

    var queue = new Queue<(int, int, int)>();
    var visited = new bool[rows, cols];
    queue.Enqueue((0, 0, 0));
    visited[0, 0] = true;

    while (queue.Count > 0)
    {
        var (x, y, dist) = queue.Dequeue();

        if (x == 70 && y == 70)
        {
            return dist;
        }

        foreach (var (dx, dy) in directions)
        {
            int newX = x + dx;
            int newY = y + dy;

            if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && !visited[newX, newY] && grid[newX, newY] != '#')
            {
                queue.Enqueue((newX, newY, dist + 1));
                visited[newX, newY] = true;
            }
        }
    }
    
    return -1;
}

// Part 2
for (int next = 1024; next < bytes.Length; next++)
{
    grid[bytes[next].Item2, bytes[next].Item1] = '#';
    if (FindShortestPath() == -1)
    {
        Console.WriteLine($"{bytes[next].Item1},{bytes[next].Item2}");
        break;
    }
}