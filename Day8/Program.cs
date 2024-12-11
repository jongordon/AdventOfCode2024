// Read the input file
string[] lines = File.ReadAllLines("input.txt");
var height = lines.Length;
var width = lines[0].Length;

// Store antenna positions by frequency
var antennasByFreq = new Dictionary<char, List<(int x, int y)>>();

// Find all antennas and group them by frequency
for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        char c = lines[y][x];
        if (c != '.')
        {
            if (!antennasByFreq.ContainsKey(c))
                antennasByFreq[c] = new List<(int, int)>();
            antennasByFreq[c].Add((x, y));
        }
    }
}

// Set to store unique antinode positions
var antinodes = new HashSet<(int x, int y)>();

// For each frequency
foreach (var freq in antennasByFreq.Keys)
{
    var antennas = antennasByFreq[freq];
    
    // If there's more than one antenna of this frequency,
    // all antenna positions of this frequency are antinodes
    if (antennas.Count > 1)
    {
        foreach (var ant in antennas)
        {
            antinodes.Add(ant);
        }
    }
    
    // Check each pair of antennas with the same frequency
    for (int i = 0; i < antennas.Count; i++)
    {
        for (int j = i + 1; j < antennas.Count; j++)
        {
            var ant1 = antennas[i];
            var ant2 = antennas[j];

            // Calculate the vector between antennas
            int dx = ant2.x - ant1.x;
            int dy = ant2.y - ant1.y;

            // Check all points on the line between and beyond the antennas
            // Find the GCD to get the smallest step size that will hit all grid points
            int gcd = GCD(Math.Abs(dx), Math.Abs(dy));
            if (gcd == 0) gcd = 1;
            
            // Unit vector in grid coordinates
            int ux = dx / gcd;
            int uy = dy / gcd;

            // Extend line in both directions until we hit map boundaries
            // Start from ant1 and go backwards
            int x = ant1.x;
            int y = ant1.y;
            while (IsInBounds(x - ux, y - uy, width, height))
            {
                x -= ux;
                y -= uy;
                antinodes.Add((x, y));
            }

            // Start from ant2 and go forwards
            x = ant2.x;
            y = ant2.y;
            while (IsInBounds(x + ux, y + uy, width, height))
            {
                x += ux;
                y += uy;
                antinodes.Add((x, y));
            }
        }
    }
}

Console.WriteLine($"{antinodes.Count}");

// Optional: Print the map with antinodes for visualization
for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        if (antinodes.Contains((x, y)))
            Console.Write('#');
        else
            Console.Write(lines[y][x]);
    }
    Console.WriteLine();
}

static bool IsInBounds(int x, int y, int width, int height)
{
    return x >= 0 && x < width && y >= 0 && y < height;
}

static int GCD(int a, int b)
{
    while (b != 0)
    {
        int temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}
