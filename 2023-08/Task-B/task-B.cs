using System;
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
            ProcessCase();
        }

        void ProcessCase()
        {
            string line = _reader.ReadLine();
            int count = _reader.ReadInt();

            var sb = new StringBuilder(line);

            for (int i = 0; i < count; i++)
            {
                string str = _reader.ReadLine();
                var pieces = str.Split(' ');
                int start = int.Parse(pieces[0]);
                for (int j = 0; j < pieces[2].Length; j++)
                    sb[start + j - 1] = pieces[2][j];
            }

            _writer.WriteLine(sb.ToString());
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
