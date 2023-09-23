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
        List<Tuple<TimeOnly, TimeOnly>> times = new();
        bool fail = false;

        int n = int.Parse(reader.ReadLine());
        for (int i = 0; i < n; i++)
        {
            string input = reader.ReadLine();

            if (fail)
                continue;

            string[] values = input.Split('-');

            (bool isValid, TimeOnly time1) = Parse(values[0]);
            if (!isValid)
            {
                fail = true;
                continue;
            }

            (isValid, TimeOnly time2) = Parse(values[1]);
            if (!isValid)
            {
                fail = true;
                continue;
            }

            if (time2 < time1)
            {
                fail = true;
                continue;
            }

            times.Add(Tuple.Create(time1, time2));
        }

        if (fail)
        {
            writer.WriteLine("NO");
            return;
        }

        var ordered = times.OrderBy(x => x.Item1)
                           .ToArray();

        for (int i = 0; i < ordered.Length - 1; i++)
        {
            if (ordered[i].Item2 >= ordered[i + 1].Item1)
            {
                writer.WriteLine("NO");
                return;
            }
        }

        writer.WriteLine("YES");
    }

    static (bool isValid, TimeOnly time) Parse(string input)
    {
        string[] values = input.Split(':');
        int hour = int.Parse(values[0]);
        int min = int.Parse(values[1]);
        int sec = int.Parse(values[2]);

        if (hour > 23 || min > 59 || sec > 59)
            return (false, new TimeOnly());

        return (true, new TimeOnly(hour, min, sec));
    }
}
