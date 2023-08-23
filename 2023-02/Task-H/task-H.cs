using System;
using System.Collections.Generic;
using System.Data;
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
            int result = ProcessCase();
            _writer.WriteLine(result);
        }

        int ProcessCase()
        {
            var field = ReadField();
            var figures = ReadFigures();

            var permutations = new List<int[]>();
            MakePermutations(new int[figures.Length], 0, permutations);

            int result = int.MaxValue;
            foreach (var permutation in permutations)
            {
                var grade = Evaluate(field, figures, permutation);
                if (grade == -1)
                    continue;
                result = Math.Min(result, grade);
            }

            return result == int.MaxValue ? -1 : result;
        }

        int Evaluate(Field field, Figure[] figures, int[] order)
        {
            var fields = new Queue<Field>();
            fields.Enqueue(field);

            foreach (int fi in order)
            {
                int count = fields.Count;
                for (int i = 0; i < count; i++)
                {
                    foreach (var resultField in fields.Dequeue().PlaceFigure(figures[fi]))
                        fields.Enqueue(resultField);
                }
            }

            if (fields.Count == 0)
                return -1;

            return fields.Min(f => f.FilledCellsCount);
        }

        Field ReadField()
        {
            var field = new char[Constants.FieldSize][];

            for (int i = 0; i < Constants.FieldSize; i++)
                field[i] = _reader.ReadLine().ToArray();

            return new Field(field);
        }

        Figure[] ReadFigures()
        {
            int count = _reader.ReadInt();
            var figures = new Figure[count];

            for (int i = 0; i < count; i++)
                figures[i] = ReadFigure();

            return figures;
        }

        Figure ReadFigure()
        {
            int size = _reader.ReadInt();
            var field = new char[size][];

            for (int i = 0; i < size; i++)
                field[i] = _reader.ReadLine().ToArray();

            return new Figure(field);
        }

        static void MakePermutations(int[] permutation, int position, List<int[]> results)
        {
            if (position == permutation.Length)
            {
                results.Add((int[])permutation.Clone());
                return;
            }

            for (int i = 0; i < permutation.Length; i++)
            {
                var index = Array.IndexOf(permutation, i, 0, position);
                if (index != -1)
                    continue;
                permutation[position] = i;
                MakePermutations(permutation, position + 1, results);
            }
        }
    }

    class Figure
    {
        readonly char[][] _field;

        public Figure(char[][] field)
        {
            _field = field;
        }

        public char this[int y, int x]
        {
            get => _field[y][x];
            set => _field[y][x] = value;
        }

        public int Width => _field[0].GetLength(0);

        public int Height => _field.GetLength(0);
    }

    class Field
    {
        readonly char[][] _field;

        public Field(char[][] field)
        {
            _field = field;
        }

        private Field(Field field)
        {
            _field = field._field.Select(line => line.ToArray()).ToArray();
        }

        public char this[int y, int x]
        {
            get => _field[y][x];
            set => _field[y][x] = value;
        }

        public int Width => _field[0].GetLength(0);

        public int Height => _field.GetLength(0);

        public int FilledCellsCount => _field.Sum(line => line.Count(cell => cell == Constants.FilledCell));

        public Field[] PlaceFigure(Figure figure)
        {
            var fields = new List<Field>();

            for (int y = 0; y <= Height - figure.Height; y++)
            {
                for (int x = 0; x <= Width - figure.Width; x++)
                {
                    var point = new Point(x, y);
                    if (TryToPlaceFigure(figure, point, out Field? field))
                        fields.Add(field);
                }
            }

            return fields.ToArray();
        }

        bool TryToPlaceFigure(Figure figure, Point point, out Field? field)
        {
            if (!FigureFit(figure, point))
            {
                field = null;
                return false;
            }

            field = new Field(this);
            field.PlaceFigureWithoutChecking(figure, point);
            field.ClearFilledLines();

            return true;
        }

        bool FigureFit(Figure figure, Point point)
        {
            for (int y = 0; y < figure.Height; y++)
                for (int x = 0; x < figure.Width; x++)
                    if (figure[y, x] == Constants.FilledCell && this[point.Y + y, point.X + x] == Constants.FilledCell)
                        return false;

            return true;
        }

        void PlaceFigureWithoutChecking(Figure figure, Point point)
        {
            for (int y = 0; y < figure.Height; y++)
                for (int x = 0; x < figure.Width; x++)
                    if (figure[y, x] == Constants.FilledCell)
                        this[point.Y + y, point.X + x] = Constants.FilledCell;
        }

        void ClearFilledLines()
        {
            var rowNumbers = GetFilledRowNumbers().ToArray();
            var colNumbers = GetFilledColumnNumbers().ToArray();

            foreach (var num in rowNumbers)
                ClearRow(num);

            foreach (var num in colNumbers)
                ClearColumn(num);

        }

        IEnumerable<int> GetFilledRowNumbers()
        {
            for (int y = 0; y < Height; y++)
            {
                if (_field[y].All(cell => cell == Constants.FilledCell))
                    yield return y;
            }
        }

        IEnumerable<int> GetFilledColumnNumbers()
        {
            for (int x = 0; x < Width; x++)
            {
                if (_field.All(line => line[x] == Constants.FilledCell))
                    yield return x;
            }
        }

        void ClearRow(int number)
        {
            for (int i = 0; i < Width; i++)
                _field[number][i] = Constants.EmptyCell;
        }

        void ClearColumn(int number)
        {
            for (int i = 0; i < Height; i++)
                _field[i][number] = Constants.EmptyCell;
        }
    }

    record Point(int X, int Y);

    static class Constants
    {
        public const int FieldSize = 8;
        public const char EmptyCell = '.';
        public const char FilledCell = '*';
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
