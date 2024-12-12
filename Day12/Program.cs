using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Read the grid from input.txt
        string[] lines = File.ReadAllLines("input.txt");
        char[,] grid = new char[lines.Length, lines[0].Length];

        // Convert input to 2D array
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                grid[i, j] = lines[i][j];
            }
        }

        // Keep track of visited cells
        bool[,] visited = new bool[grid.GetLength(0), grid.GetLength(1)];
        
        // Dictionary to store results for each region
        Dictionary<char, List<(int size, int perimeter)>> regions = new Dictionary<char, List<(int size, int perimeter)>>();

        // Process each cell in the grid
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (!visited[i, j])
                {
                    var (size, perimeter) = ExploreRegion(grid, visited, i, j);
                    char letter = grid[i, j];
                    
                    if (!regions.ContainsKey(letter))
                        regions[letter] = new List<(int size, int perimeter)>();
                    
                    regions[letter].Add((size, perimeter));
                }
            }
        }

        // Print totals for each letter and total cost
        long totalCost = 0;
        foreach (var region in regions)
        {
            totalCost += region.Value.Sum(r => (long)r.size * r.perimeter);
        }

        Console.WriteLine($"Total Cost = {totalCost}");
    }

    static (int size, int perimeter) ExploreRegion(char[,] grid, bool[,] visited, int row, int col)
    {
        // Check bounds and if already visited
        if (row < 0 || row >= grid.GetLength(0) || col < 0 || col >= grid.GetLength(1) ||
            visited[row, col])
            return (0, 0);

        char currentLetter = grid[row, col];
        // This line was causing the issue - we were checking against currentLetter instead of the actual cell
        if (currentLetter == '.') // Only return if it's not a valid letter (assuming '.' represents empty space)
            return (0, 0);

        visited[row, col] = true;
        int size = 1;
        int perimeter = 0;

        // Check all four sides for perimeter calculation
        perimeter += IsEdgeOrDifferentLetter(grid, row - 1, col, currentLetter) ? 1 : 0;
        perimeter += IsEdgeOrDifferentLetter(grid, row + 1, col, currentLetter) ? 1 : 0;
        perimeter += IsEdgeOrDifferentLetter(grid, row, col - 1, currentLetter) ? 1 : 0;
        perimeter += IsEdgeOrDifferentLetter(grid, row, col + 1, currentLetter) ? 1 : 0;

        // Recursively explore adjacent cells
        var directions = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
        foreach (var (dr, dc) in directions)
        {
            var nextRow = row + dr;
            var nextCol = col + dc;
            
            // Only explore if it's the same letter
            if (nextRow >= 0 && nextRow < grid.GetLength(0) && 
                nextCol >= 0 && nextCol < grid.GetLength(1) && 
                grid[nextRow, nextCol] == currentLetter)
            {
                var (additionalSize, additionalPerimeter) = ExploreRegion(grid, visited, nextRow, nextCol);
                size += additionalSize;
                perimeter += additionalPerimeter;
            }
        }

        return (size, perimeter);
    }

    static bool IsEdgeOrDifferentLetter(char[,] grid, int row, int col, char letter)
    {
        if (row < 0 || row >= grid.GetLength(0) || col < 0 || col >= grid.GetLength(1))
            return true;
        return grid[row, col] != letter;
    }
}
