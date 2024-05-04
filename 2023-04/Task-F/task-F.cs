using System;
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
            var input = _reader.ReadInts();
            
            var nodes = input
                .Select((x, i) => new Node { Value = x, Position = i })
                .ToArray();
            var ordered = nodes
                .OrderBy(n => n.Value)
                .ToArray();
            var positions = nodes
                .OrderBy(n => n.Position)
                .ToArray();

            var sb = new StringBuilder();
            var index = 0;

            foreach (var node in ordered)
            {
                var stepsToLeft = CountStepsToLeft(node, index, count);
                var stepsToRight = CountStepsToRight(node, index, count);

                sb.Append(GetStepsPresentation(stepsToLeft, stepsToRight));
                sb.Append('!');

                index = node.Position;
                count--;

                if (index == count)
                    index = 0;
                else
                    UpdatePositions(positions, index);
            }

            _writer.WriteLine(sb.ToString());
        }

        private static string GetStepsPresentation(int stepsToLeft, int stepsToRight)
            => stepsToLeft <= stepsToRight
                ? new string('L', stepsToLeft)
                : new string('R', stepsToRight);

        private static int CountStepsToRight(Node node, int index, int count)
            => node.Position < index
                ? index - node.Position
                : index + (count - node.Position);

        private static int CountStepsToLeft(Node node, int index, int count) 
            => node.Position < index
                ? count - index + node.Position
                : node.Position - index;

        private static void UpdatePositions(Node[] positions, int index)
        {
            var i = positions.Length;
            while (positions[--i].Position > index)
            {
                positions[i].Position--;
            }
        }
    }

    internal class Node
    {
        public int Value { get; init; }
        public int Position { get; set; }
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