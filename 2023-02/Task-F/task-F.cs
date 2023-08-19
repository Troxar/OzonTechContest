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
                ProcessCase();
            }
        }

        private void ProcessCase()
        {
            int count = _reader.ReadInt();
            var line = _reader.ReadLine();

            var state = ProcessLine(line, count);
            var result = ProcessState(state);

            _writer.WriteLine(result);
        }

        private bool[] ProcessLine(string line, int count)
        {
            var state = new bool[count];

            foreach (var portion in line.Split(','))
            {
                if (portion.Contains('-'))
                {
                    var pieces = portion.Split('-');
                    var left = int.Parse(pieces[0]);
                    var right = int.Parse(pieces[1]);

                    for (int i = left; i <= right; i++)
                        state[i - 1] = true;
                }
                else
                    state[int.Parse(portion) - 1] = true;
            }

            return state;
        }

        private string ProcessState(bool[] state)
        {
            var portions = new List<string>();
            var left = 0;
            var right = 0;
            
            for (int i = 0; i < state.Length; i++)
            {
                bool printed = state[i];
                if (!printed)
                {
                    if (left == 0)
                        left = i + 1;
                    right = i + 1;
                }

                if (left != 0 && (printed || i == state.Length - 1))
                {
                    if (left == right)
                        portions.Add(left.ToString());
                    else
                        portions.Add($"{left}-{right}");
                    left = right = 0;
                }
            }

            return string.Join(',', portions);
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
