// Input
using System.Text;

string input = File.ReadAllText($"{Directory.GetCurrentDirectory()}/input.txt");

// Part 1
int sum = 0;
for (int i = 0; i < input.Length-4; ++i)
{
    if (input.Substring(i, 4).Equals("mul("))
    {
        i += 4;
        StringBuilder firstNum = new StringBuilder();
        StringBuilder secondNum = new StringBuilder();
        bool foundComma = false;
        bool isValid = true;

        // Parse until we find the closing parenthesis
        while (i < input.Length && input[i] != ')')
        {
            char c = input[i];
            
            if (char.IsDigit(c))
            {
                if (!foundComma)
                    firstNum.Append(c);
                else
                    secondNum.Append(c);
            }
            else if (c == ',')
            {
                if (foundComma || firstNum.Length == 0)
                {
                    isValid = false;
                    break;
                }
                foundComma = true;
            }
            else if (!char.IsWhiteSpace(c))
            {
                isValid = false;
                break;
            }
            i++;
        }

        // Validate the multiplication
        if (isValid && foundComma && 
            firstNum.Length > 0 && firstNum.Length <= 3 &&
            secondNum.Length > 0 && secondNum.Length <= 3)
        {
            if (int.TryParse(firstNum.ToString(), out int a) &&
                int.TryParse(secondNum.ToString(), out int b))
            {
                sum += a * b;
            }
        }
    }
}
System.Console.WriteLine(sum);

// Part 2
sum = 0;
bool mulEnabled = true; // Start with mul enabled

for (int i = 0; i < input.Length - 3; i++)
{
    // Check for "do(" pattern
    if (input[i] == 'd' && input[i + 1] == 'o' && input[i + 2] == '(')
    {
        mulEnabled = true;
        continue;
    }
    // Check for "don't(" pattern
    else if (i < input.Length - 5 && 
             input[i] == 'd' && input[i + 1] == 'o' && 
             input[i + 2] == 'n' && input[i + 3] == '\'' && 
             input[i + 4] == 't' && input[i + 5] == '(')
    {
        mulEnabled = false;
        continue;
    }
    
    // Only process mul instructions if enabled
    if (mulEnabled && input[i] == 'm' && input[i + 1] == 'u' && 
        input[i + 2] == 'l' && input[i + 3] == '(')
    {
        i += 4; // Move past "mul("
        StringBuilder firstNum = new StringBuilder();
        StringBuilder secondNum = new StringBuilder();
        bool foundComma = false;
        bool isValid = true;

        // Parse until we find the closing parenthesis
        while (i < input.Length && input[i] != ')')
        {
            char c = input[i];
            
            if (char.IsDigit(c))
            {
                if (!foundComma)
                    firstNum.Append(c);
                else
                    secondNum.Append(c);
            }
            else if (c == ',')
            {
                if (foundComma || firstNum.Length == 0)
                {
                    isValid = false;
                    break;
                }
                foundComma = true;
            }
            else if (!char.IsWhiteSpace(c))
            {
                isValid = false;
                break;
            }
            i++;
        }

        // Validate the multiplication
        if (isValid && foundComma && 
            firstNum.Length > 0 && firstNum.Length <= 3 &&
            secondNum.Length > 0 && secondNum.Length <= 3)
        {
            if (int.TryParse(firstNum.ToString(), out int a) &&
                int.TryParse(secondNum.ToString(), out int b))
            {
                sum += a * b;
            }
        }
    }
}

Console.WriteLine(sum);
