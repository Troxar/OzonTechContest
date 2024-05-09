using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContestConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            var reader = Console.In;
            var writer = Console.Out;
            var executor = new Executor(reader, writer);
            executor.Execute();
        }
    }

    public class Executor
    {
        private readonly TextReader _reader;
        private readonly TextWriter _writer;

        public Executor(TextReader reader, TextWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public void Execute()
        {
            var caseCount = _reader.ReadInt();
            for (var i = 0; i < caseCount; i++)
            {
                ProcessCase();
            }
        }

        private void ProcessCase()
        {
            var count = _reader.ReadInt();
            var max = _reader.ReadInt();
            var dict = new Dictionary<int, int>();
            var ids = new List<int>(count);

            ReadInput(count, dict, ids);
            var checksToPrint = GetChecksToPrint(max, dict, ids);

            _writer.WriteLine(checksToPrint.Count);
            foreach (var ch in checksToPrint)
                _writer.Write(ch);
        }

        private static ChecksToPrint GetChecksToPrint(int max, Dictionary<int, int> dict, List<int> ids)
        {
            var checksToPrint = new ChecksToPrint();
            var check = checksToPrint.StartNewCheck();

            foreach (var id in ids)
            {
                if (check.Total == max)
                    check = checksToPrint.StartNewCheck();

                var cost = dict[id];
                while (cost > 0)
                {
                    var costToAdd = Math.Min(cost, max - check.Total);
                    check.AddItem(id, costToAdd);
                    cost -= costToAdd;

                    if (cost == 0)
                        continue;

                    check = checksToPrint.StartNewCheck();
                }
            }

            return checksToPrint;
        }

        private void ReadInput(int count, Dictionary<int, int> dict, List<int> ids)
        {
            for (var i = 0; i < count; i++)
            {
                var input = _reader.ReadInts();
                var id = input[0];
                var cost = input[1];
                if (dict.ContainsKey(id))
                {
                    dict[id] += cost;
                }
                else
                {
                    dict[id] = cost;
                    ids.Add(id);
                }
            }
        }
    }

    internal class ChecksToPrint : IEnumerable<Check>
    {
        private readonly List<Check> _checks = new();

        internal Check StartNewCheck()
        {
            var check = new Check();
            _checks.Add(check);
            return check;
        }

        public IEnumerator<Check> GetEnumerator()
        {
            return _checks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal int Count => _checks.Count;
    }

    internal class Check
    {
        private readonly List<CheckItem> _items = new();
        internal int Total { get; private set; }

        internal void AddItem(int id, int cost)
        {
            _items.Add(new CheckItem { Id = id, Cost = cost });
            Total += cost;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_items.Count.ToString());

            foreach (var item in _items)
                sb.AppendLine(item.ToString());

            return sb.ToString();
        }
    }

    internal class CheckItem
    {
        internal int Id;
        internal int Cost;

        public override string ToString()
        {
            return $"{Id} {Cost}";
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