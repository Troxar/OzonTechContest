using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
                string result = ProcessLine(_reader.ReadLine());
                _writer.WriteLine(result);
            }
        }

        private static string ProcessLine(string line)
        {
            var regex = new Regex("^[A-Z][0-9]{1,2}[A-Z]{2}");
            var sb = new StringBuilder();

            while (line.Length > 0)
            {
                var match = regex.Match(line);
                if (!match.Success)
                    return "-";

                sb.Append(match.Value + " ");
                line = line.Substring(match.Value.Length);
            }

            return sb.ToString().Trim();
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
