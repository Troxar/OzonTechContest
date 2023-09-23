using System;
using System.Collections.Generic;
using System.IO;

public class Program
{
    static TextReader reader;
    static TextWriter writer;

    const char EMPTY = '.';
    const char FILLED = '*';
    const char OPTIONAL = '?';
    const string SUCCESS = "YES";
    const string FAIL = "NO";

    static Pattern[] PATTERNS = GetPatterns();

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
        Field field = new Field(n, m);

        List<int> result = new();

        foreach (Pattern pattern in PATTERNS)
        {
            int count = field.ApplyPattern(pattern);
            for (int i = 0; i < count; i++)
            {
                result.Add(pattern.Count);
            }
        }

        if (field.Contains(FILLED))
        {
            writer.WriteLine(FAIL);
        }
        else
        {
            writer.WriteLine(SUCCESS);
            writer.WriteLine(string.Join(' ', result));
        }
    }

    class Field
    {
        public int N { get; set; }
        public int M { get; set; }
        public char[,] Array { get; private set; } = null!;

        public Field(int n, int m)
        {
            N = n + 2;
            M = m + 2;
            FillArray();
        }

        private void FillArray()
        {
            Array = new char[N, M];
            foreach (int y in new int[] { 0, N - 1 })
            {
                for (int x = 0; x < M; x++)
                {
                    Array[y, x] = EMPTY;
                }
            }

            for (int y = 1; y < N - 1; y++)
            {
                string input = reader.ReadLine();
                char[] symbols = input.ToCharArray();

                int x = 0;
                Array[y, x++] = EMPTY;

                foreach (char symbol in symbols)
                {
                    Array[y, x++] = symbol;
                }

                Array[y, x] = EMPTY;
            }
        }

        public int ApplyPattern(Pattern pattern)
        {
            int count = 0;

            if (pattern.N > N || pattern.M > M)
                return count;

            for (int y = 0; y <= (N - pattern.N); y++)
            {
                for (int x = 0; x <= (M - pattern.M); x++)
                {
                    if (IsAreaMatchesPattern(y, x, pattern))
                    {
                        count++;
                        ClearAreaWithPattern(y, x, pattern);
                    }
                }
            }

            return count;
        }

        private bool IsAreaMatchesPattern(int Y, int X, Pattern pattern)
        {
            for (int y = 0; y < pattern.N; y++)
            {
                for (int x = 0; x < pattern.M; x++)
                {
                    if (pattern.Array[y, x] == OPTIONAL)
                        continue;

                    if (pattern.Array[y, x] != Array[Y + y, X + x])
                        return false;
                }
            }

            return true;
        }

        private void ClearAreaWithPattern(int Y, int X, Pattern pattern)
        {
            for (int y = 1; y < pattern.N - 1; y++)
            {
                for (int x = 1; x < pattern.M - 1; x++)
                {
                    if (pattern.Array[y, x] == OPTIONAL)
                        continue;

                    Array[Y + y, X + x] = EMPTY;
                }
            }
        }

        public bool Contains(char symbol)
        {
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < M; x++)
                {
                    if (Array[y, x] == symbol)
                        return true;
                }
            }

            return false;
        }
    }

    class Pattern
    {
        public int N { get; }
        public int M { get; }
        public int Count { get; }
        public char[,] Array { get; } = null!;

        public Pattern(int n, int m, int count, char[,] array)
        {
            N = n;
            M = m;
            Count = count;
            Array = array;
        }

        public Pattern Rotate()
        {
            char[,] array = new char[M, N];
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < M; x++)
                {
                    array[x, N - 1 - y] = Array[y, x];
                }
            }

            return new Pattern(M, N, Count, array);
        }
    }

    static Pattern[] GetPatterns()
    {
        Pattern pattern1 = GetPattern1();

        Pattern pattern2 = GetPattern2();
        Pattern pattern2_R1 = pattern2.Rotate();
        Pattern pattern2_R2 = pattern2_R1.Rotate();
        Pattern pattern2_R3 = pattern2_R2.Rotate();

        Pattern pattern3 = GetPattern3();
        Pattern pattern3_R1 = pattern3.Rotate();
        Pattern pattern3_R2 = pattern3_R1.Rotate();
        Pattern pattern3_R3 = pattern3_R2.Rotate();

        Pattern pattern4 = GetPattern4();
        Pattern pattern4_R1 = pattern4.Rotate();
        Pattern pattern4_R2 = pattern4_R1.Rotate();
        Pattern pattern4_R3 = pattern4_R2.Rotate();

        Pattern[] patterns = new[] {
                                    pattern1,
                                    pattern2,
                                    pattern2_R1,
                                    pattern2_R2,
                                    pattern2_R3,
                                    pattern3,
                                    pattern3_R1,
                                    pattern3_R2,
                                    pattern3_R3,
                                    pattern4,
                                    pattern4_R1,
                                    pattern4_R2,
                                    pattern4_R3
                                    };
        return patterns;
    }

    static Pattern GetPattern1()
    {
        // ...
        // .*.
        // ...

        Pattern pattern = new Pattern(3, 3, 1, new[,] { { EMPTY, EMPTY, EMPTY },
                                                        { EMPTY, FILLED, EMPTY },
                                                        { EMPTY, EMPTY, EMPTY }
        });

        return pattern;
    }

    static Pattern GetPattern2()
    {
        // ...?
        // .*..
        // .**.
        // ....

        Pattern pattern = new Pattern(4, 4, 3, new[,] { { EMPTY, EMPTY, EMPTY, OPTIONAL },
                                                        { EMPTY, FILLED, EMPTY, EMPTY },
                                                        { EMPTY, FILLED, FILLED, EMPTY },
                                                        { EMPTY, EMPTY, EMPTY, EMPTY }
        });

        return pattern;
    }

    static Pattern GetPattern3()
    {
        // ...??
        // .*.??
        // .*...
        // .***.
        // .....

        Pattern pattern = new Pattern(5, 5, 5, new[,] { { EMPTY, EMPTY, EMPTY, OPTIONAL, OPTIONAL, },
                                                        { EMPTY, FILLED, EMPTY, OPTIONAL, OPTIONAL },
                                                        { EMPTY, FILLED, EMPTY, EMPTY, EMPTY },
                                                        { EMPTY, FILLED, FILLED, FILLED, EMPTY },
                                                        { EMPTY, EMPTY, EMPTY, EMPTY, EMPTY }
        });

        return pattern;
    }

    static Pattern GetPattern4()
    {
        // ...???
        // .*.???
        // .*.???
        // .*....
        // .****.
        // ......

        Pattern pattern = new Pattern(6, 6, 7, new[,] { { EMPTY, EMPTY, EMPTY, OPTIONAL, OPTIONAL, OPTIONAL },
                                                        { EMPTY, FILLED, EMPTY, OPTIONAL, OPTIONAL, OPTIONAL },
                                                        { EMPTY, FILLED, EMPTY, OPTIONAL, OPTIONAL, OPTIONAL },
                                                        { EMPTY, FILLED, EMPTY, EMPTY, EMPTY, EMPTY },
                                                        { EMPTY, FILLED, FILLED, FILLED, FILLED, EMPTY },
                                                        { EMPTY, EMPTY, EMPTY, EMPTY, EMPTY, EMPTY }
        });

        return pattern;
    }
}
