using System;
using System.Collections.Generic;
using System.IO;

namespace KizhiPart2
{
    public class Interpreter
    {
        private readonly TextWriter _writer;
        private readonly Dictionary<string, long> _variables = new Dictionary<string, long>();
        private readonly Dictionary<string, string> _functions = new Dictionary<string, string>();
        private string _uploadedCode;
        private bool _isSettingCode;
        private readonly Dictionary<string, Action<string, string>> _operators =
            new Dictionary<string, Action<string, string>>();

        private string _lastDefinedFunc = "";

        public Interpreter(TextWriter writer)
        {
            _writer = writer;
            _operators.Add("set", (key, value) => _variables.Add(key, int.Parse(value)));
            _operators.Add("sub",
                (key, value) => ExecuteSavely(key, delegate { _variables[key] -= int.Parse(value); }));
            _operators.Add("print",
                (key, value) => ExecuteSavely(key, delegate { _writer.WriteLine(_variables[key]); }));
            _operators.Add("rem",
                (key, value) => ExecuteSavely(key, delegate { _variables.Remove(key); }));
            _operators.Add("call", (key, value) => ExecuteUploadedCode(_functions[key]));
            _operators.Add("def", delegate(string key, string value)
            {
                _functions.Add(key, "");
                _lastDefinedFunc = key;
            });
        }

        public void ExecuteLine(string command)
        {
            var splitedCommand = command.Split(' ');
            if (splitedCommand[0] == "end")
                _isSettingCode = false;
            else if (splitedCommand[0] == "run")
                ExecuteUploadedCode(_uploadedCode);
            else if (splitedCommand[1] == "code")
                _isSettingCode = true;
            else if (_isSettingCode)
                _uploadedCode = command;
        }
    

        private void ExecuteUploadedCode(string uploadedCode)
        {
            var commands = uploadedCode.Split('\n');
            foreach (var line in commands)
            {
                var splitedLine = line.Split(' ');
                if (line.StartsWith(" "))
                    _functions[_lastDefinedFunc] += line.Substring(4) + '\n';
                else if (splitedLine[0] != "")
                {
                    var value = splitedLine.Length > 2 ? splitedLine[2] : null;
                    _operators[splitedLine[0]](splitedLine[1], value);
                }
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