using System;
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
        _ = reader.ReadLine();
        string input = reader.ReadLine();
        int n = int.Parse(input.Split(' ').First());

        Row[] rows = ReadRows(n);
        int[] clicks = ReadClicks();

        foreach (int click in clicks)
        {
            rows = GetSortedRows(rows, click);
        }

        foreach (Row row in rows)
        {
            writer.WriteLine(row);
        }

        writer.WriteLine();
    }

    private class Row
    {
        public int Index { get; set; }
        public int[] Values { get; }

        public Row(int index, params int[] values)
        {
            Index = index;
            Values = values;
        }

        public override string ToString()
        {
            return String.Join(' ', Values);
        }
    }

    private static Row[] ReadRows(int count)
    {
        Row[] rows = new Row[count];

        for (int i = 0; i < count; i++)
        {
            int[] values = reader.ReadLine()
                                 .Split(' ')
                                 .Select(x => int.Parse(x))
                                 .ToArray();
            rows[i] = new Row(i, values);
        }

        return rows;
    }

    private static int[] ReadClicks()
    {
        _ = reader.ReadLine();
        string input = reader.ReadLine();

        return input.Split(' ')
                    .Select(x => int.Parse(x))
                    .ToArray();
    }

    private static Row[] GetSortedRows(Row[] rows, int column)
    {
        int i = 0, j = 0;
        return rows.Select(x => new { Index = i++, Value = x.Values[column - 1] })
                   .OrderBy(x => x.Value)
                   .Select(x => new Row(j++, rows[x.Index].Values))
                   .ToArray();
    }
}
