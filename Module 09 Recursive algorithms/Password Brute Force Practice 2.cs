using System;
using System;
using System.Collections.Generic;

public class CaseAlternatorTask
{
    public static List<string> AlternateCharCases(string source)
    {
        if (string.IsNullOrEmpty(source))
            return new List<string> { source };

        var result = new List<string>();
        var chars = source.ToCharArray();

        Generate(chars, 0, result);
        return result;
    }

    private static void Generate(char[] data, int index, List<string> result)
    {
        if (index == data.Length)
        {
            result.Add(new string(data));
            return;
        }

        char current = data[index];

        if (char.IsLetter(current) && char.IsLower(current))
        {
            Generate(data, index + 1, result);
            char upper = char.ToUpper(current);
            if (upper != current)
            {
                data[index] = upper;
                Generate(data, index + 1, result);
                data[index] = current;
            }
        }
        else
        {
            Generate(data, index + 1, result);
        }
    }
}
