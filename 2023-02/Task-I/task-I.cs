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
                var table = ProcessCase();
                _writer.WriteLine(table.ToString());
            }
        }

        Table ProcessCase()
        {
            var definition = ReadDefinition();
            var table = BuildTree(definition);
            return table;
        }

        Table BuildTree(string[] definition)
        {
            var elements = new Stack<Element>();
            Element? current = null;

            foreach (string line in definition)
            {
                if (line == string.Empty)
                    continue;

                if (line.StartsWith('/'))
                {
                    if (elements.Count() > 0)
                        current = elements.Pop();
                    continue;
                }

                var next = GetElement(line);
                if (current is not null)
                {
                    current.Add(next);
                    elements.Push(current);
                }
                current = next;
            }

            var table = current as Table;
            if (table is null)
                return new Table();

            table.CalculateHeight();
            table.CalculateWidth();

            return table;
        }

        string[] ReadDefinition()
        {
            var sb = new StringBuilder();
            int count = _reader.ReadInt();
            
            for (int i = 0; i < count; i++)
                sb.Append(_reader.ReadLine());

            sb.Replace(" ", string.Empty);
            sb.Replace("<", string.Empty);

            return sb.ToString().Split('>');
        }

        Element GetElement(string elementName) => elementName switch
        {
            "table" => new Table(),
            "tr" => new Row(),
            "td" => new Cell(),
            _ => throw new ArgumentException("Invalid element name", nameof(elementName))
        };
    }

    abstract class Element
    {
        public abstract void Add(Element element);
        public int Height { get; protected set; }
        public int Width { get; protected set; }
    }

    class Table : Element
    {
        public List<Row> Rows { get; } = new();
        Dictionary<int, int> _columnWidths = new();

        public override void Add(Element element)
        {
            Rows.Add((Row)element);
        }

        public int CalculateHeight()
        {
            Height = Rows.Sum(row => row.CalculateHeight() + 1) + 1;
            return Height;
        }

        public int CalculateWidth()
        {
            Width = Rows[0].Cells.Select((cell, i) => ColumnWidth(i) + 1).Sum(w => w) + 1;
            return Width;
        }

        public int ColumnWidth(int i)
        {
            if (!_columnWidths.ContainsKey(i))
                _columnWidths[i] = Rows.Max(row => row.Cells[i].CalculateWidth());
            return _columnWidths[i];
        }

        public override string ToString()
        {
            var canvas = new Canvas(Height, Width);
            canvas.Draw(this, 0, 0);
            return canvas.ToString();
        }
    }

    class Row : Element
    {
        public List<Cell> Cells { get; } = new();

        public override void Add(Element element)
        {
            Cells.Add((Cell)element);
        }

        public int CalculateHeight()
        {
            Height = Cells.Max(cell => cell.CalculateHeight());
            return Height;
        }

        public int CalculateWidth()
        {
            Width = Cells.Sum(cell => cell.CalculateWidth() + 1) + 1;
            return Width;
        }
    }

    class Cell : Element
    {
        public Table? Table { get; private set; }

        public override void Add(Element element)
        {
            Table = (Table)element;
        }

        public int CalculateHeight()
        {
            Height = Table is null ? 1 : Table.CalculateHeight();
            return Height;
        }

        public int CalculateWidth()
        {
            Width = Table is null ? 1 : Table.CalculateWidth();
            return Width;
        }
    }

    class Canvas
    {
        readonly char[,] _canvas;

        public Canvas(int height, int width)
        {
            _canvas = new char[height, width];
        }

        public void Draw(Table table, int y, int x)
        {
            DrawVertical(x, y, y + table.Height - 1);
            DrawHorizontal(y, x, x + table.Width - 1);

            int x1 = x;
            for (int i = 0; i < table.Rows[0].Cells.Count; i++)
            {
                int x2 = x1 + table.ColumnWidth(i) + 1;
                DrawVertical(x2, y, y + table.Height - 1);

                int y1 = y;
                foreach (var row in table.Rows)
                {
                    int y2 = y1 + row.Height + 1;
                    DrawHorizontal(y2, x1, x2);
                    Draw(row.Cells[i], y1 + 1, x1 + 1);
                    y1 = y2;
                }

                x1 = x2;
            }
        }

        void Draw(Cell cell, int y, int x)
        {
            if (cell.Table is not null)
                Draw(cell.Table, y, x);
        }

        void DrawHorizontal(int y, int x1, int x2)
        {
            for (int i = x1; i <= x2; i++)
                _canvas[y, i] = _canvas[y, i] == 0
                    ? Constants.Floor
                    : Constants.Corner;
        }

        void DrawVertical(int x, int y1, int y2)
        {
            for (int i = y1; i <= y2; i++)
                _canvas[i, x] = _canvas[i, x] == 0
                    ? Constants.Wall
                    : Constants.Corner;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int y = 0; y < _canvas.GetLength(0); y++)
            {
                if (y > 0)
                    sb.Append(Environment.NewLine);
                for (int x = 0; x < _canvas.GetLength(1); x++)
                    sb.Append(_canvas[y, x]);
            }

            sb.Replace((char)0, Constants.Empty);

            return sb.ToString();
        }
    }

    static class Constants
    {
        public const char Corner = '+';
        public const char Wall = '|';
        public const char Floor = '-';
        public const char Empty = '.';
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
