using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
                ProcessCase();
            }
        }

        private void ProcessCase()
        {
            int count = _reader.ReadInt();
            var hash = new HashSet<string>();

            for (int i = 0; i < count; i++)
                hash.Add(ProcessLine(_reader.ReadLine()));

            _writer.WriteLine(hash.Count);
        }

        private string ProcessLine(string line)
        {
            var sb = new StringBuilder();
            var prev = (char)0;
            var count = 0;

            foreach (var current in line)
            {
                if (current == prev)
                    count++;
                else
                {
                    count = 1;
                    prev = current;
                }

                if (count <= 2)
                    sb.Append(current);
            }

            return sb.ToString();
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
