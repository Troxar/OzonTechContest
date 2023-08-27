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
                ProcessCase();
        }

        void ProcessCase()
        {
            int count = _reader.ReadInt();
            var ints = _reader.ReadInts();

            var nodes = new Dictionary<int, Node>();
            int i = 0;

            while (i < ints.Length)
            {
                int id = ints[i];
                if (!nodes.ContainsKey(id))
                    nodes.Add(id, new Node { Id = id });
                var node = nodes[id];

                for (int j = 0; j < ints[i + 1]; j++)
                {
                    int child_id = ints[i + 2 + j];
                    if (!nodes.ContainsKey(child_id))
                        nodes.Add(child_id, new Node { Id = child_id });
                    var child = nodes[child_id];
                    child.Parent = node;
                }

                i += 2 + ints[i + 1];
            }

            _writer.WriteLine(nodes.Values.First(x => x.Parent is null).Id);
        }
    }

    class Node
    {
        internal int Id;
        internal Node Parent;
        
        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is Node node 
                && node.GetHashCode() == GetHashCode();
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
