using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContestConsoleApp
{
    internal class Program
    {
        static void Main()
        {
            var reader = Console.In;
            var writer = Console.Out;
            var executor = new Executor(reader, writer);
            executor.Execute();
        }
    }

    public class Executor
    {
        TextReader _reader;
        TextWriter _writer;

        public Executor(TextReader reader, TextWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public void Execute()
        {
            int caseCount = _reader.ReadInt();

            for (int i = 0; i < caseCount; i++)
            {
                bool isCorrect = SequenceIsCorrect(_reader.ReadInts());
                _writer.WriteLine(isCorrect ? "YES" : "NO");
            }
        }

        private bool SequenceIsCorrect(IEnumerable<int> ints)
        {
            var dict = new Dictionary<int, int>
            {
                { 1, 4 },
                { 2, 3 },
                { 3, 2 },
                { 4, 1 }
            };

            foreach (int value in ints)
            {
                if (!dict.ContainsKey(value))
                    return false;
                dict[value]--;
            }

            return dict.Values.All(x => x == 0);
        }
    }

    internal static class Extensions
    {
        internal static int ReadInt(this TextReader reader)
        {
            return int.Parse(reader.ReadLine());
        }

        internal static int[] ReadInts(this TextReader reader)
        {
            return reader.ReadLine()
                .Split(' ')
                .Select(x => int.Parse(x))
                .ToArray();
        }
    }
}
