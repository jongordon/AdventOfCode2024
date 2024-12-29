// Input
string[] input = File.ReadAllLines("input.txt");
long regA = long.Parse(input[0].Split(" ")[2]);
long regB = long.Parse(input[1].Split(" ")[2]);
long regC = long.Parse(input[2].Split(" ")[2]);
int[] program = input[4].Split(" ")[1].Split(',').Select(int.Parse).ToArray();

// Part 1
var result = ParseProgram(ref regA, ref regB, ref regC);

List<long> ParseProgram(ref long regA, ref long regB, ref long regC)
{
    int ip = 0;
    List<long> outs = new List<long>();
    for (; ip < program.Length;)
    {
        int oc = program[ip++];
        int op = program[ip++];

        switch (oc)
        {
            case 0: // adv
                long num = regA;
                double den = Math.Pow(2, ComboOperand(op));
                long res = (long)Math.Truncate(num / den);
                regA = res;
                break;

            case 1: // bxl
                regB ^= op;
                break;

            case 2: // bst
                regB = ComboOperand(op) % 8;
                break;

            case 3: // jnz
                if (regA != 0) ip = op;
                continue;

            case 4: // bxc
                regB ^= regC;
                break;

            case 5: // out
                outs.Add(ComboOperand(op) % 8);
                break;

            case 6: // bdv
                num = regA;
                den = Math.Pow(2, ComboOperand(op));
                res = (long)Math.Truncate(num / den);
                regB = res;
                break;

            case 7: // cdv
                num = regA;
                den = Math.Pow(2, ComboOperand(op));
                res = (long)Math.Truncate(num / den);
                regC = res;
                break;
        }
    }

    return outs;
}

bool first = true;
foreach (long o in result)
{
    if (!first)
    {
        Console.Write(",");
    }
    Console.Write(o);
    first = false;
}
Console.WriteLine();

long ComboOperand(long op)
{
    if (op <= 3) return op;
    if (op == 4) return regA;
    if (op == 5) return regB;
    if (op == 6) return regC;
    return 0;   // Won't occur
}

// Part 2
long answer = 0;
var programList = program.Select(x => (long)x).ToList();

for (var i = program.Length - 1; i >= 0; i--)
{
    var increment = (long)Math.Pow(8, i);
    var incrementCounter = 0;

    while (true)
    {
        regA = answer + increment * incrementCounter;
        regB = 0;
        regC = 0;

        var res = ParseProgram(ref regA, ref regB, ref regC);

        // Check if the output matches the program from the current position to the end
        bool match = true;
        for (int j = i; j < programList.Count; j++)
        {
            if (j >= res.Count || res[j] != programList[j])
            {
                match = false;
                break;
            }
        }

        if (match)
        {
            break;
        }

        incrementCounter++;
    }
    answer += increment * incrementCounter;
}

Console.WriteLine(answer);