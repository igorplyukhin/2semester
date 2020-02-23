using System;
using System.Collections.Generic;
using System.IO;

namespace KizhiPart3
{
    public class Debugger
    {
        private readonly TextWriter _writer;
        private readonly Interpreter _interpreter;
        private readonly Dictionary<string, Action<string>> _mainOperators =
            new Dictionary<string, Action<string>>();

        private bool _isSettingCode;
        private string _uploadedCode;
        public Debugger(TextWriter writer)
        {
            _writer = writer;
            _interpreter = new Interpreter(writer);
            _mainOperators.Add("set code", s => _isSettingCode = true);
            _mainOperators.Add("end set code", s => _isSettingCode = false);
            _mainOperators.Add("run", s => _interpreter.ExecuteLine(_uploadedCode));
        }

        public void ExecuteLine(string command)
        {
            if (_isSettingCode)
            {
                _uploadedCode = command;
                _isSettingCode = false;
            }
            else
            {
                if (command.StartsWith("add break"))
                    command = "add break";
                
                _mainOperators[command](null);
            }
        }
    }
    
    public class Interpreter
    {
        private readonly TextWriter _writer;
        private readonly Dictionary<string, long> _variables = new Dictionary<string, long>();
        private readonly Dictionary<string, string> _functions = new Dictionary<string, string>();
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
            ExecuteUploadedCode(command);
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