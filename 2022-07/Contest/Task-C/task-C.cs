using System;
using System.IO;

public class Program
{
    static TextReader reader;
    static TextWriter writer;

    public static void Main()
    {
        reader = Console.In;
        writer = Console.Out;
        
        SolveTheTask();
    }

    private static void SolveTheTask()
    {
        string input = reader.ReadLine();
        string[] values = input.Split(' ');

        int n = int.Parse(values[0]);
        int q = int.Parse(values[1]);

        int[] notifications = new int[n + 1];
        int number = 1;

        for (int i = 0; i < q; i++)
        {
            input = reader.ReadLine();
            values = input.Split(' ');

            RequestType type = (RequestType)int.Parse(values[0]);
            int id = int.Parse(values[1]);

            if (type == RequestType.Send)
            {
                notifications[id] = number++;
            }
            else if (type == RequestType.ShowLastest)
            {
                writer.WriteLine(Math.Max(notifications[0], notifications[id]));
            }
        }
    }

    enum RequestType
    {
        Send = 1,
        ShowLastest = 2
    }
}
