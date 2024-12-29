// Input
string[] input = File.ReadAllLines("input.txt");
long regA = long.Parse(input[0].Split(" ")[2]);
long regB = long.Parse(input[1].Split(" ")[2]);
long regC = long.Parse(input[2].Split(" ")[2]);
int[] program = input[4].Split(" ")[1].Split(',').Select(int.Parse).ToArray();

// Part 1
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

bool first = true;
foreach (long o in outs)
{
    if (!first)
    {
        Console.Write(",");
    }
    Console.Write(o);
    first = false;
}

long ComboOperand(long op)
{
    if (op <= 3) return op;
    if (op == 4) return regA;
    if (op == 5) return regB;
    if (op == 6) return regC;
    return 0;   // Won't occur
}