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
            var hands = new Hand[count];

            for (int i = 0; i < count; i++)
            {
                string line = _reader.ReadLine();
                var pieces = line.Split(' ');
                var hand = new Hand();
                hand.Card1 = new Card(pieces[0][0], pieces[0][1]);
                hand.Card2 = new Card(pieces[1][0], pieces[1][1]);
                hands[i] = hand;
            }

            var result = new List<Card>();

            foreach (var value in Constants.Values)
            {
                foreach (var suit in Constants.Suits)
                {
                    var card = new Card(value, suit);
                    if (hands.Any(h => h.Card1 == card || h.Card2 == card))
                        continue;

                    var evals = hands.Select((h, i) => Tuple.Create(i, h.Evaluate(card)))
                        .OrderByDescending(h => h.Item2)
                        .ThenBy(h => h.Item1)
                        .ToArray();
                    if (evals[0].Item1 == 0)
                        result.Add(card);
                }
            }

            _writer.WriteLine(result.Count);

            foreach (var card in result)
                _writer.WriteLine(card);
        }
    }

    class Hand
    {
        public Card Card1;
        public Card Card2;

        public int Evaluate(Card card3)
        {
            int value1 = Constants.ValuesWeights[Card1.Value];
            int value2 = Constants.ValuesWeights[Card2.Value];
            int value3 = Constants.ValuesWeights[card3.Value];

            if (Card1.Value == Card2.Value && Card1.Value == card3.Value)
                return value1 * 10000;

            if (Card1.Value == Card2.Value || Card1.Value == card3.Value)
                return value1 * 100;

            if (Card2.Value == card3.Value)
                return value2 * 100;

            return Math.Max(value1, Math.Max(value2, value3));
        }
    }

    record class Card
    {
        public char Value;
        public char Ace;

        public Card(char value, char ace)
        {
            Value = value;
            Ace = ace;
        }

        public override string ToString()
        {
            return $"{Value}{Ace}";
        }
    }

    static class Constants
    {
        public static char[] Suits = new[] { 'S', 'C', 'D', 'H' };
        public static char[] Values = new[] { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
        public static Dictionary<char, int> ValuesWeights = Values
            .Select((v, i) => Tuple.Create(v, i + 1))
            .ToDictionary(k => k.Item1, v => v.Item2);
    }

    internal static class Extensions
    {
        internal static int ReadInt(this TextReader reader)
        {
            return int.Parse(reader.ReadLine());
        }
    }
}
