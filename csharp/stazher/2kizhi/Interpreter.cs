using System;
using System.Collections.Generic;
using System.IO;

namespace KizhiPart1
{
    public class Interpreter
    {
        private readonly TextWriter _writer;
        private readonly Dictionary<string, long> _variables = new Dictionary<string, long>();
        private readonly Dictionary<string, string> _functions = new Dictionary<string, string>();
        private List<string> _settedCode = new List<string>();
        private bool _isSettingCode;

        public Interpreter(TextWriter writer)
        {
            _writer = writer;
        }

        public void ExecuteLine(string command)
        {
            var splitedCommand = command.Split(' ');
            if (splitedCommand[1] == "code" || _isSettingCode)
            {
                _isSettingCode = true;
                _settedCode.Add(command);
            }
            else if (splitedCommand[0] == "end")
            {
                _isSettingCode = false;
            }
            else if (splitedCommand[0] == "run")
            {
                foreach (var line in _settedCode)
                {
                    var splitedLine = line.Split(' ');
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
                            
                    }
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