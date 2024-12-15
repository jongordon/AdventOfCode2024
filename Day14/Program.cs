// Input
using System.Runtime.CompilerServices;

string[] input = File.ReadAllLines("input.txt");
int cols = 101, rows = 103;
int[,] map = new int[cols, rows];
List<Guard> guards = new List<Guard>();

foreach (var inStr in input)
{
    string[] elements = inStr.Split(new[] { '=', ' ', ',' });
    Guard guard;
    guard.position = new Coord { x = int.Parse(elements[1]), y = int.Parse(elements[2]) };
    guard.vector = new Coord { x = int.Parse(elements[4]), y = int.Parse(elements[5]) };
    guards.Add(guard);
}

// Part 1
for (int i = 0; i < guards.Count; ++i)
{
    // Calculate 100 moves
    int x = guards[i].position.x + (guards[i].vector.x * 100);
    x %= cols;
    int y = guards[i].position.y + (guards[i].vector.y * 100);
    y %= rows;

    // Check out of bounds
    if (x < 0) x = cols + x;
    if (y < 0) y = rows + y;

    // Place guard in end position
    ++map[x, y];
}

// Calculate safety factor
int[] quadrants = new int[4];
int q = 0;
int discarded = 0;
for (int y = 0; y < rows; y++)
{
    if (y < (rows / 2)) q = 0;
    else q = 2;

    for (int x = 0; x < cols; x++)
    { 
        if (x == (cols / 2) || y == (rows / 2))
        { 
            if (map[x, y] > 0) discarded += map[x, y];  // For checking we've evaluated all 500
            continue; 
        }
        else if (x > (cols / 2) && q == 0) q = 1;
        else if (x > (cols / 2) && q == 2) q = 3;

        quadrants[q] += map[x, y];
    }
}
int safetyFactor = quadrants[0];
for (q = 1; q < 4; ++q)
{
    safetyFactor *= quadrants[q];
}
Console.WriteLine(safetyFactor);
Console.WriteLine(quadrants[0] + quadrants[1] + quadrants[2] + quadrants[3] + discarded);   // Check we have all 500

struct Coord
{
    public int x;
    public int y;
}

struct Guard
{
    public Coord position;
    public Coord vector;
}