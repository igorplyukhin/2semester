using System;
using System.Collections.Generic;
using System.IO;

namespace KizhiPart1
{
    public class Interpreter
    {
        private readonly TextWriter _writer;
        private readonly Dictionary<string, long> _variables = new Dictionary<string, long>();

        public Interpreter(TextWriter writer)
        {
            _writer = writer;
        }

        public void ExecuteLine(string command)
        {
            var splitedCommand = command.Split(' ');
            switch (splitedCommand[0])
            {
                case "set":
                    _variables.Add(splitedCommand[1], int.Parse(splitedCommand[2]));
                    break;
                case "sub":
                    ExecuteSavely(splitedCommand[1],
                        delegate { _variables[splitedCommand[1]] -= int.Parse(splitedCommand[2]); });
                    break;
                case "print":
                    ExecuteSavely(splitedCommand[1], delegate { _writer.WriteLine(_variables[splitedCommand[1]]); });
                    break;
                case "rem":
                    ExecuteSavely(splitedCommand[1], delegate { _variables.Remove(splitedCommand[1]); });
                    break;
            }
        }

        private void ExecuteSavely(string keyToCheck, Action command)
        {
            if (_variables.ContainsKey(keyToCheck))
                command();
            else
                _writer.WriteLine("Переменная отсутствует в памяти");
        }
    }
}