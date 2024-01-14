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
            int caseCount = _reader.ReadInt();
            for (int i = 0; i < caseCount; i++)
                ProcessCase();
        }

        void ProcessCase()
        {
            int count = _reader.ReadInt();
            var range = Enumerable.Range(15, 16);

            for (int i = 0; i < count; i++)
            {
                string line = _reader.ReadLine();
                var pieces = line.Split(' ');
                int limit = int.Parse(pieces[1]);

                if (pieces[0] == ">=")
                    range = range.Where(x => x >= limit);
                else
                    range = range.Where(x => x <= limit);

                _writer.WriteLine(range.FirstOrDefault(-1));
            }

            _writer.WriteLine();
        }
    }

    internal static class Extensions
    {
        internal static int ReadInt(this TextReader reader)
        {
            return int.Parse(reader.ReadLine());
        }
    }
}
