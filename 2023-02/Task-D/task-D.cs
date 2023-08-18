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
                int memberCount = _reader.ReadInt();
                int[] times = _reader.ReadInts();
                string result = ProcessTimes(times);
                _writer.WriteLine(result);
            }
        }

        string ProcessTimes(IEnumerable<int> times)
        {
            var members = times.Select((time, i) => new Member(i + 1, time))
                .OrderBy(m => m.Time)
                .ToArray();

            int position = 1;
            int count = 0;
            Member prev = members.First();

            foreach (var member in members)
            {
                if (member.Time > prev.Time + 1)
                {
                    position += count;
                    count = 0;
                }

                member.Position = position;
                count++;
                prev = member;
            }

            return string.Join(' ', members.OrderBy(x => x.Number)
                .Select(x => x.Position)) + " ";
        }

        class Member
        {
            internal readonly int Number;
            internal readonly int Time;
            internal int Position;

            internal Member(int number, int time)
            {
                Number = number;
                Time = time;
            }
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
