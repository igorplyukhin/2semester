using System;
using System.Collections.Generic;
using System.IO;

namespace KizhiPart2
{
    public class Interpreter
    {
        private readonly TextWriter _writer;
        private readonly Dictionary<string, long> _variables = new Dictionary<string, long>();
        private readonly Dictionary<string, List<string>> _functions = new Dictionary<string, List<string>>();
        private List<string> _settedCode = new List<string>();
        private bool _isSettingCode;

        public Interpreter(TextWriter writer)
        {
            _writer = writer;
        }

        public void ExecuteLine(string command)
        {
            var splitedCommand = command.Split(' ');
            if (splitedCommand[0] == "end")
            {
                _isSettingCode = false;
            }
            else if (splitedCommand[0] == "run")
            {
                ExecuteSettedCode(_settedCode);
            }
            else if (splitedCommand[1] == "code")
            {
                _isSettingCode = true;
            }
            else if (_isSettingCode)
            {
                _settedCode.Add(command);
            }
        }


        private void ExecuteSettedCode(List<string> settedCode)
        {
            var lastDefinedFunc = "";
            foreach (var line in settedCode)
            {
                var splitedLine = line.Split(' ');
                if (line[0] == ' ')
                    _functions[lastDefinedFunc].Add(line.Substring(4));
                else
                    switch (splitedLine[0])
                    {
                        case "set":
                            _variables.Add(splitedLine[1], int.Parse(splitedLine[2]));
                            break;
                        case "sub":
                            ExecuteSavely(splitedLine[1],
                                delegate { _variables[splitedLine[1]] -= int.Parse(splitedLine[2]); });
                            break;
                        case "print":
                            ExecuteSavely(splitedLine[1],
                                delegate { _writer.WriteLine(_variables[splitedLine[1]]); });
                            break;
                        case "rem":
                            ExecuteSavely(splitedLine[1], delegate { _variables.Remove(splitedLine[1]); });
                            break;
                        case "def":
                            _functions.Add(splitedLine[1], new List<string>());
                            lastDefinedFunc = splitedLine[1];
                            break;
                        case "call":
                            ExecuteSettedCode(_functions[splitedLine[1]]);
                            throw new Exception(String.Join(" ", _functions[splitedLine[1]]));
                            break;
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