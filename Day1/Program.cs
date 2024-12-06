// Input
List<int> colA = new List<int>();
List<int> colB = new List<int>();

foreach (string line in File.ReadLines($"{Directory.GetCurrentDirectory()}/input.txt"))
{
    string[] split = line.Split(' ');
    colA.Add(int.Parse(split[0]));
    colB.Add(int.Parse(split[split.Length - 1]));
}

// Part 1
colA.Sort();
colB.Sort();

int sumDiff = 0;
for (int i = 0; i < colA.Count; i++)
{
    sumDiff += Math.Abs(colA[i] - colB[i]);
}
Console.WriteLine(sumDiff);

