using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class Program
{
    static TextReader reader;
    static TextWriter writer;

    public static void Main()
    {
        reader = Console.In;
        writer = Console.Out;

        int n = int.Parse(reader.ReadLine());
        for (int i = 0; i < n; i++)
        {
            SolveTheTask();
        }
    }

    private static void SolveTheTask()
    {
        const string NEGATIVE_RESULT = "NO";
        const string POSITIVE_RESULT = "YES";

        Regex regex = new Regex(@"(?=.{2,24}$)^[A-Z\d_][A-Z\d_-]*$");
        var cache = new HashSet<string>();

        int n = int.Parse(reader.ReadLine());
        for (int i = 0; i < n; i++)
        {
            string value = reader.ReadLine().ToUpper();
            if (cache.Contains(value))
            {
                writer.WriteLine(NEGATIVE_RESULT);
            }
            else
            {
                cache.Add(value);
                writer.WriteLine(regex.IsMatch(value) ? POSITIVE_RESULT : NEGATIVE_RESULT);
            }
        }

        writer.WriteLine();
    }
}
