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

            if (i < n - 1)
            {
                writer.WriteLine();
            }
        }
    }

    private static void SolveTheTask()
    {
        int n = int.Parse(reader.ReadLine());
        List<Dev> devs = new();

        string input = reader.ReadLine();
        string[] values = input.Split(' ');

        for (int i = 0; i < values.Length; i++)
        {
            devs.Add(new Dev { Number = i + 1, Level = int.Parse(values[i]) });
        }

        for (int i = 0; i < n; i++)
        {
            Dev current = devs[i];

            if (current.Level == 0)
                continue;

            Dev bestCandidate = current;
            int minDiff = int.MaxValue;

            for (int j = i + 1; j < n; j++)
            {
                Dev candidate = devs[j];

                if (candidate.Level == 0)
                    continue;

                int diff = Math.Abs(candidate.Level - current.Level);
                if (diff < minDiff)
                {
                    bestCandidate = candidate;
                    minDiff = diff;
                }
            }

            writer.WriteLine($"{current.Number} {bestCandidate.Number}");
            bestCandidate.Level = 0;
        }
    }

    class Dev
    {
        public int Number { get; set; }
        public int Level { get; set; }
    }
}
