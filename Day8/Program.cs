// Input
string[] lines = File.ReadAllLines("input.txt");
var height = lines.Length;
var width = lines[0].Length;

// Part 1

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
            double distance = Math.Sqrt(dx * dx + dy * dy);

            // Calculate unit vector
            double ux = dx / distance;
            double uy = dy / distance;

            // Calculate antinode positions
            // First antinode: extend beyond ant1 by distance
            int antinode1X = (int)Math.Round(ant1.x - ux * distance);
            int antinode1Y = (int)Math.Round(ant1.y - uy * distance);

            // Second antinode: extend beyond ant2 by distance
            int antinode2X = (int)Math.Round(ant2.x + ux * distance);
            int antinode2Y = (int)Math.Round(ant2.y + uy * distance);

            // Add antinodes if they're within bounds
            if (IsInBounds(antinode1X, antinode1Y, width, height))
                antinodes.Add((antinode1X, antinode1Y));
            if (IsInBounds(antinode2X, antinode2Y, width, height))
                antinodes.Add((antinode2X, antinode2Y));
        }
    }
}

Console.WriteLine($"{antinodes.Count}");

static bool IsInBounds(int x, int y, int width, int height)
{
    return x >= 0 && x < width && y >= 0 && y < height;
}
