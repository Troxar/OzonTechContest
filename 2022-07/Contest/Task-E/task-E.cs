using System;
using System.Collections.Generic;
using System.IO;

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
        _ = reader.ReadLine();
        string input = reader.ReadLine();
        string[] values = input.Split(' ');

        HashSet<string> tasks = new();
        string prev = String.Empty;

        foreach (string value in values)
        {
            if (!String.IsNullOrEmpty(prev) && (value == prev))
                continue;

            if (tasks.Contains(value))
            {
                writer.WriteLine("NO");
                return;
            }
            
            tasks.Add(value);
            prev = value;
        }

        writer.WriteLine("YES");
    }
}
