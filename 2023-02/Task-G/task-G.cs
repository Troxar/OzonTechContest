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
            var counts = _reader.ReadInts();
            int windowCount = counts[0];
            int patientCount = counts[1];

            var reserves = _reader.ReadInts();
            var result = ProcessReserves(reserves, windowCount);

            _writer.WriteLine(result);
        }

        private string ProcessReserves(int[] reserves, int windowCount)
        {
            var patients = reserves.Select((r, i) => new Patient { Number = i + 1, Window = r })
                .ToArray();
            var reserved = new HashSet<int>();

            foreach (var patient in patients.OrderBy(p => p.Window))
            {
                if (!Reserve(reserved, patient, windowCount, patient.Window - 1)
                    && !Reserve(reserved, patient, windowCount, patient.Window)
                    && !Reserve(reserved, patient, windowCount, patient.Window + 1))
                    return "x";
            }

            return string.Join(string.Empty, patients.Select(p => StateToChar(p.State)));
        }

        char StateToChar(int state) => state switch
        {
            0 => '0',
            1 => '+',
            -1 => '-',
            _ => '?'
        };

        bool Reserve(HashSet<int> reserved, Patient patient, int windowCount, int window)
        {
            if (window < 1
                || window > windowCount
                || reserved.Contains(window))
                return false;

            patient.State = window - patient.Window;
            reserved.Add(window);
            return true;
        }

        class Patient
        {
            public int Number;
            public int Window;
            public int State;
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
