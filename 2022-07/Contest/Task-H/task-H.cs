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
        string input = reader.ReadLine();
        string[] values = input.Split(' ');

        int n = int.Parse(values[0]);
        int m = int.Parse(values[1]);

        char[,] field = new char[n, m];
        Dictionary<char, int> stats = new();

        for (int y = 0; y < n; y++)
        {
            input = reader.ReadLine();
            char[] chars = input.ToCharArray();

            for (int x = 0; x < m; x++)
            {
                char ch = chars[x];
                if (ch != '.')
                {
                    if (!stats.ContainsKey(ch))
                        stats.Add(ch, 1);
                    else
                        stats[ch]++;
                }

                field[y, x] = ch;
            }
        }

        if (stats.Count == 1)
        {
            writer.WriteLine("YES");
            return;
        }

        bool result = CheckTheField(field, n, m, stats);
        writer.WriteLine(result ? "YES" : "NO");
    }

    static bool CheckTheField(char[,] field, int n, int m, Dictionary<char, int> stats)
    {
        Queue<Point> queue = new();

        for (int y = 0; y < n; y++)
        {
            for (int x = 0; x < m; x++)
            {
                char current = field[y, x];
                if (current == '.')
                    continue;

                queue.Enqueue(new Point { X = x, Y = y });
                while (queue.Count > 0)
                {
                    Point point = queue.Dequeue();
                    if (field[point.Y, point.X] == '.')
                        continue;

                    field[point.Y, point.X] = '.';
                    stats[current]--;

                    // left
                    int y1 = point.Y;
                    int x1 = point.X - 2;
                    if ((x1 >= 0) && (field[y1, x1] == current))
                        queue.Enqueue(new Point { X = x1, Y = y1 });

                    // right
                    y1 = point.Y;
                    x1 = point.X + 2;
                    if ((x1 < m) && (field[y1, x1] == current))
                        queue.Enqueue(new Point { X = x1, Y = y1 });

                    // up left
                    y1 = point.Y - 1;
                    x1 = point.X - 1;
                    if ((y1 >= 0) && (x1 >= 0) && (field[y1, x1] == current))
                        queue.Enqueue(new Point { X = x1, Y = y1 });

                    // up right
                    y1 = point.Y - 1;
                    x1 = point.X + 1;
                    if ((y1 >= 0) && (x1 < m) && (field[y1, x1] == current))
                        queue.Enqueue(new Point { X = x1, Y = y1 });

                    // down left
                    y1 = point.Y + 1;
                    x1 = point.X - 1;
                    if ((y1 < n) && (x1 >= 0) && (field[y1, x1] == current))
                        queue.Enqueue(new Point { X = x1, Y = y1 });

                    // down right
                    y1 = point.Y + 1;
                    x1 = point.X + 1;
                    if ((y1 < n) && (x1 < m) && (field[y1, x1] == current))
                        queue.Enqueue(new Point { X = x1, Y = y1 });
                }

                if (stats[current] > 0)
                    return false;
            }
        }

        return true;
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
