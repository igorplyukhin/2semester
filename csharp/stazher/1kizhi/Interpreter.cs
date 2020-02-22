using System;
using System.Collections.Generic;
using System.IO;

namespace KizhiPart1
{
    public class Interpreter
    {
        private readonly TextWriter _writer;
        private readonly Dictionary<string, long> _variables = new Dictionary<string, long>();
        private readonly Dictionary<string, Action<string, string>> possibleCommands =
            new Dictionary<string, Action<string, string>>();

        public Interpreter(TextWriter writer)
        {
            _writer = writer;
            possibleCommands.Add("set", (key, value) => _variables.Add(key, int.Parse(value)));
            possibleCommands.Add("sub",
                (key, value) => ExecuteSavely(key, delegate { _variables[key] -= int.Parse(value); }));
            possibleCommands.Add("print",
                (key, value) => ExecuteSavely(key, delegate { _writer.WriteLine(_variables[key]); }));
            possibleCommands.Add("rem",
                (key, value) => ExecuteSavely(key, delegate { _variables.Remove(key); }));
        }

        public void ExecuteLine(string command)
        {
            var splitedCommand = command.Split(' ');
            var value = splitedCommand.Length > 2 ? splitedCommand[2] : null;
            possibleCommands[splitedCommand[0]](splitedCommand[1], value);
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