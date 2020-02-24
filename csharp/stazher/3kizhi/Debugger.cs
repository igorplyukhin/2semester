using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KizhiPart3
{
    public class Debugger
    {
        private readonly TextWriter _writer;
        private readonly Interpreter _interpreter;
        private readonly Dictionary<string, Action<string>> _mainOperators =
            new Dictionary<string, Action<string>>();
        private readonly HashSet<long> _breakPoints = new HashSet<long>();

        private bool _isSettingCode;
        private string _uploadedCode;
        public Debugger(TextWriter writer)
        {
            _writer = writer;
            _interpreter = new Interpreter(writer);
            _mainOperators.Add("set code", s => _isSettingCode = true);
            _mainOperators.Add("end set code", s => _isSettingCode = false);
            _mainOperators.Add("run", s => _interpreter.ExecuteUploadedCode(_uploadedCode));
            _mainOperators.Add("add break", s => _breakPoints.Add(long.Parse(s)));
            _mainOperators.Add("print mem", s => _writer.WriteLine(_interpreter.Variables));
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
                string arg = null;
                if (command.StartsWith("add break"))
                {
                    arg = command.Split()[3];
                    command = "add break";
                }

                _mainOperators[command.Trim()](arg);
            }
        }
    }
    
    public class Interpreter
    {
        private readonly TextWriter _writer;
        private readonly Dictionary<string, Tuple<long, long>> _variables = new Dictionary<string, Tuple<long, long>>();
        private readonly Dictionary<string, string> _functions = new Dictionary<string, string>();
        private readonly Dictionary<string, Action<string, string, long>> _operators =
            new Dictionary<string, Action<string, string, long>>();

        private string _lastDefinedFunc = "";

        public string Variables
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var e in _variables)
                    sb.Append(e.Key + ' ' + e.Value.Item1 + ' ' + e.Value.Item2 + '\n');

                return sb.ToString();
            }
        }

        public Interpreter(TextWriter writer)
        {
            _writer = writer;
            _operators.Add("set", 
                (key, value, n) => _variables.Add(key, Tuple.Create(long.Parse(value), n)));
            _operators.Add("sub",
                (key, value, n) => ExecuteSavely(key, delegate
                {
                    _variables[key] = Tuple.Create(_variables[key].Item1 - long.Parse(value), n) ;
                }));
            _operators.Add("print",
                (key, value, n) => ExecuteSavely(key, delegate { _writer.WriteLine(_variables[key].Item1); }));
            _operators.Add("rem",
                (key, value, n) => ExecuteSavely(key, delegate { _variables.Remove(key); }));
            _operators.Add("call", (key, value,n) => ExecuteUploadedCode(_functions[key]));
            _operators.Add("def", delegate(string key, string value, long n)
            {
                _functions.Add(key, "");
                _lastDefinedFunc = key;
            });
        }

        public void ExecuteUploadedCode(string uploadedCode)
        {
            var commands = uploadedCode.Split('\n');
            for (var i = 0; i < commands.Length; i++)
            {
                var line = commands[i];
                var splitedLine = line.Split(' ');
                if (line.StartsWith(" "))
                    _functions[_lastDefinedFunc] += line.Substring(4) + '\n';
                else if (splitedLine[0] != "")
                {
                    var value = splitedLine.Length > 2 ? splitedLine[2] : null;
                    _operators[splitedLine[0]](splitedLine[1], value, i);
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