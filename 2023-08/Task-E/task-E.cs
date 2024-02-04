using System;
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
            int caseCount = 1;
            for (int i = 0; i < caseCount; i++)
                ProcessCase();
        }

        void ProcessCase()
        {
            var ints = _reader.ReadInts();
            int count = ints[1];

            ints = _reader.ReadInts();
            var friends = ints.Select((x, i) => new Friend { Number = i + 1, Count = x })
                .OrderByDescending(f => f.Count)
                .ThenByDescending(f => f.Number)
                .ToArray();

            foreach (var friend in friends)
            {
                if (friend.Count >= count)
                {
                    _writer.WriteLine(-1);
                    return;
                }

                friend.Gift = count;
                count--;
            }

            _writer.Write(string.Join(' ', friends.OrderBy(f => f.Number).Select(f => f.Gift)));
            _writer.WriteLine(" ");
        }
    }

    class Friend
    {
        public int Number;
        public int Count;
        public int Gift;
    }

    internal static class Extensions
    {
        internal static int[] ReadInts(this TextReader reader)
        {
            var line = reader.ReadLine();
            return line
                .Split(' ')
                .Select(x => int.Parse(x))
                .ToArray();
        }
    }
}
