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
                var nodes = ProcessCase();
                _writer.WriteLine(NodesToString(nodes));
            }
        }

        List<Node> ProcessCase()
        {
            int count = _reader.ReadInt();
            var ints = _reader.ReadInts();

            var nodes = new List<Node>();
            Node node = new Node(int.MaxValue);

            foreach (int current in ints)
            {
                if (node.Direction == 0 && current == node.Number + 1)
                {
                    node.Direction = +1;
                    node.Duration = 1;
                }
                else if (node.Direction == 0 && current == node.Number - 1)
                {
                    node.Direction = -1;
                    node.Duration = 1;
                }
                else if (current == node.ExpectedNextNumber)
                {
                    node.Duration++;
                }
                else
                {
                    node = new Node(current);
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        string NodesToString(List<Node> nodes)
        {
            var sb = new StringBuilder();
            sb.AppendLine((nodes.Count * 2).ToString());

            foreach (var node in nodes)
                sb.Append($"{node.Number} {node.Direction * node.Duration} ");
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }

    class Node
    {
        public readonly int Number;
        public int Direction;
        public int Duration;

        public Node(int number)
        {
            Number = number;
        }

        public int ExpectedNextNumber => Direction == 0
            ? int.MaxValue
            : Number + (Direction * (Duration + 1));
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
