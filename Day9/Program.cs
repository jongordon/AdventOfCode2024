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
        UpdateCheckSum(diskMap[emptyIndex], emptyIndex);
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
            UpdateCheckSum(diskMap[i], i);
        }
    }
    return index;
}

void UpdateCheckSum(int value, int index)
{
    if (value > 0)
    {
        checkSum += value * index;
    }
}

void PrintDiskMap()
{
    Console.WriteLine(string.Concat(diskMap.Select(x => x == -1 ? "." : x.ToString())));
}


// Part 2
diskMap.Clear();

CreateDiskMap(input, diskMap);

// Start from the right (highest ID) and work backwards
for (int i = diskMap.Count - 1; i >= 0; --i)
{
    int currentId = diskMap[i];
    if (currentId >= 0)
    {
        int length = 0;
        while (i >= 0 && diskMap[i] == currentId)
        {
            length++;
            i--;
        }
        ++i; // Account for extra decrement in last loop

        // We now have the rightmost file, its length, and its start pos - find a gap to the left for it
        int newStart = FindFirstGapLeft(length, i);
        if (newStart == -1)
        {
            // No gap found, move to the next file
            continue;
        }

        // Move file, deleting original
        for (int j = 0; j < length; ++j)
        {
            diskMap[newStart + j] = currentId;
            diskMap[i + j] = -1;
        }
    }
}

// Calculate checksum
checkSum = 0;
for (int i = 0; i < diskMap.Count; ++i)
{
    UpdateCheckSum(diskMap[i], i);
}
Console.WriteLine($"{checkSum}");

int FindFirstGapLeft(int requiredLength, int end)
{
    int currentGapLength = 0;
    int gapStart = -1;

    for (int i = 0; i < end; i++)
    {
        if (diskMap[i] == -1)
        {
            if (currentGapLength == 0) gapStart = i;
            if (++currentGapLength == requiredLength) return gapStart;
        }
        else
        {
            currentGapLength = 0;
        }
    }
    
    return -1; // No gap found of required length
}
