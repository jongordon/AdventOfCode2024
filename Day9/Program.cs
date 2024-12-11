// Input
string input = File.ReadAllText("input.txt");
List<int> diskMap = new List<int>();

CreateDiskMap(input, diskMap);

// Part 1
int emptyIndex = 0;
long checkSum = 0;
for (int i = diskMap.Count - 1; i >= 0; --i)
{
    if (diskMap[i] != -1)
    {
        emptyIndex = FindFirstEmptySpace(emptyIndex, diskMap.Count);
        if ((emptyIndex == diskMap.Count) || (emptyIndex == i))
        {
            // No more files
            break;
        }
        diskMap[emptyIndex] = diskMap[i];

        // Moved a file, update the checksum
        updateCheckSum(diskMap[emptyIndex], emptyIndex);
        ++emptyIndex;
    }
    diskMap.RemoveAt(i);
}
Console.WriteLine($"{checkSum}");

void CreateDiskMap(string input, List<int> diskMap)
{
    int currentId = 0;
    for (int i = 0; i < input.Length; ++i)
    {
        int length = int.Parse(input[i].ToString());
        int value = (i % 2 == 0) ? currentId++ : -1;
        diskMap.AddRange(Enumerable.Repeat(value, length));
    }
    PrintMap();
}

int FindFirstEmptySpace(int index, int count)
{
    for (int i = index; i < count; ++i)
    {
        if (diskMap[i] == -1)
        {
            return i;
        }
        else
        {
            // Not moving a file, update the checksum while we are passing over it
            updateCheckSum(diskMap[i], i);
        }
    }
    return index;
}

void updateCheckSum(int value, int index)
{
    if (value > 0)
    {
        checkSum += value * index;
    }
}

void PrintMap()
{
    Console.WriteLine(string.Concat(diskMap.Select(x => x == -1 ? "." : x.ToString())));
}