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
        const int FREE_ITEM = 3;

        _ = reader.ReadLine();
        string input = reader.ReadLine();
        int result = input.Split(' ')
                          .GroupBy(x => x)
                          .Select(x => new { Value = int.Parse(x.Key), Count = x.Count() })
                          .Sum(x => (x.Count - x.Count / FREE_ITEM) * x.Value);
        writer.WriteLine(result);
    }
}
