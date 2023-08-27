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
            string word = _reader.ReadLine();
            string result = ProcessWord(word);
            _writer.WriteLine(result);
        }

        string ProcessWord(string word)
        {
            char last1 = word[word.Length - 1];
            char last2 = word[word.Length - 2];

            if (last1 == 'y' && last2 == 'y')
                return word + "s";

            if (last1 == 'y' && !IsVowel(last2))
                return word.Remove(word.Length - 1) + "ies";

            if (last1 == 'h' && (last2 == 's' || last2 == 'c'))
                return word + "es";

            var esEndings = new[] { 's', 'z', 'x' };
            if (esEndings.Contains(last1))
                return word + "es";
            
            return word + "s";
        }

        bool IsVowel(char ch)
        {
            var vowels = new[] { 'a', 'e', 'i', 'o', 'u' };
            return vowels.Contains(ch);
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
