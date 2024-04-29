using System;
using System.Collections.Generic;
using System.IO;
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
                _writer.WriteLine("-");
            }
        }

        private void ProcessCase()
        {
            var line = _reader.ReadLine();
            var terminal = new Terminal();
            terminal.ProcessLine(line);
            terminal.Print(_writer);
        }
    }

    internal class Terminal
    {
        private readonly List<StringBuilder> _stringBuilders = new()
        {
            new StringBuilder()
        };
        
        private int _cursorLine;
        private int _cursorPosition;
        
        private StringBuilder CurrentLine => _stringBuilders[_cursorLine];
        private int LineLength => CurrentLine.Length;
        
        public void Print(TextWriter writer)
        {
            foreach (var stringBuilder in _stringBuilders)
            {
                writer.WriteLine(stringBuilder.ToString());
            }
        }
        
        internal void ProcessLine(string line)
        {
            foreach (var ch in line)
            {
                GetHandler(ch).Invoke(ch);
            }
        }
        
        private Action<char> GetHandler(char input)
            => input switch
            {
                'L' => MoveLeft,
                'R' => MoveRight,
                'U' => MoveUp,
                'D' => MoveDown,
                'B' => MoveBegin,
                'E' => MoveEnd,
                'N' => AddNewLine,
                _ => Add
            };
        
        private void MoveLeft(char input)
        {
            if (_cursorPosition > 0)
                _cursorPosition--;
        }
        
        private void MoveRight(char input)
        {
            if (_cursorPosition < LineLength)
                _cursorPosition++;
        }
        
        private void MoveUp(char input)
        {
            if (_cursorLine > 0)
            {
                _cursorLine--;
                if (_cursorPosition > LineLength)
                    _cursorPosition = LineLength;
            }
        }
        
        private void MoveDown(char input)
        {
            if (_cursorLine < _stringBuilders.Count - 1)
            {
                _cursorLine++;
                if (_cursorPosition > LineLength)
                    _cursorPosition = LineLength;
            }
        }
        
        private void MoveBegin(char input)
        {
            _cursorPosition = 0;
        }
        
        private void MoveEnd(char input)
        {
            _cursorPosition = LineLength;
        }
        
        private void AddNewLine(char input)
        {
             var line = CurrentLine.ToString(_cursorPosition, LineLength - _cursorPosition);
            CurrentLine.Remove(_cursorPosition, LineLength - _cursorPosition);
            _cursorLine++;
            _stringBuilders.Insert(_cursorLine, new StringBuilder(line));
            _cursorPosition = 0;
        }
        
        private void Add(char input)
        {
            CurrentLine.Insert(_cursorPosition, input);
            _cursorPosition++;
        }
    }

    internal static class Extensions
    {
        internal static int ReadInt(this TextReader reader)
        {
            return int.Parse(reader.ReadLine());
        }
    }
}