using System;
using System.Collections;
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

        SolveTheTask();
    }

    private static void SolveTheTask()
    {
        string input = reader.ReadLine();
        int n = int.Parse(input);

        Dictionary<string, string> dict = new();
        Dictionary<string, string> cache = new();

        for (int i = 0; i < n; i++)
        {
            input = reader.ReadLine();
            string rev = new string(input.Reverse().ToArray());
            dict.Add(rev, input);
        }

        var list = dict.Select(x => x.Key)
                       .OrderBy(x => x)
                       .ToList();

        input = reader.ReadLine();
        int q = int.Parse(input);

        for (int i = 0; i < q; i++)
        {
            input = reader.ReadLine();

            string rev = cache.GetValueOrDefault(input);
            if (rev != null)
            {
                writer.WriteLine(rev);
                continue;
            }

            string result;
            rev = new string(input.Reverse().ToArray());

            int prev, next;
            int pos = list.BinarySearch(rev);

            if (pos < 0)
            {
                pos = -pos - 1;
                prev = pos - 1;
                next = pos;
            }
            else
            {
                prev = pos - 1;
                next = pos + 1;
            }

            if (prev < 0)
            {
                result = dict[list[next]];
            }
            else if (next >= n)
            {
                result = dict[list[prev]];
            }
            else
            {
                string maxRhymed = GetMaxRhymed(rev, list[prev], list[next]);
                result = dict[maxRhymed];
            }

            cache.Add(input, result);
            writer.WriteLine(result);
        }
    }

    static string GetMaxRhymed(string input, string word1, string word2)
    {
        int cL1 = GetCommonLength(input, word1);
        int cL2 = GetCommonLength(input, word2);

        return cL1 >= cL2 ? word1 : word2;
    }

    static int GetCommonLength(string input, string word)
    {
        int max = Math.Min(input.Length, word.Length);

        for (int i = 0; i < max; i++)
        {
            if (input[i] != word[i])
                return i;
        }

        return max;
    }
}
