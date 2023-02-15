using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        const int MAX_COUNT = 5;

        int n = int.Parse(reader.ReadLine());
        var rows = new List<Row>();

        for (int i = 0; i < n; i++)
        {
            rows.Add(ReadRow());
        }

        rows.Reverse();
        var names = rows.DistinctBy(x => x.Name + x.Phone)
                        .GroupBy(x => x.Name)
                        .OrderBy(x => x.Key);

        foreach (var name in names)
        {
            writer.Write($"{name.Key}: {Math.Min(name.Count(), MAX_COUNT)}");

            foreach (Row row in name.Take(MAX_COUNT))
            {
                writer.Write($" {row.Phone}");
            }

            writer.WriteLine();
        }

        writer.WriteLine();
    }

    private class Row
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public Row(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }
    }

    private static Row ReadRow()
    {
        string input = reader.ReadLine();
        string[] values = input.Split(' ');

        return new Row(values[0], values[1]);
    }
}
