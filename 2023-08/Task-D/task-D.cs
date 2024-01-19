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
            var ints = _reader.ReadInts();
            var canvas = new char[ints[1], ints[2]];

            for (int k = 0; k < ints[0]; k++)
            {
                if (k > 0)
                    _reader.ReadLine();

                for (int y = 0; y < ints[1]; y++)
                {
                    string line = _reader.ReadLine();
                    for (int x = 0; x < line.Length; x++)
                        if (canvas[y, x] == 0 || canvas[y, x] == '.')
                            canvas[y, x] = line[x];
                }
            }

            for (int y = 0; y < canvas.GetLength(0); y++)
            {
                for (int x = 0; x < canvas.GetLength(1); x++)
                    _writer.Write(canvas[y, x]);
                _writer.WriteLine();
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
