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

        public Interpreter(TextWriter writer)
        {
            _writer = writer;
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
            var lastDefinedFunc = "";
            var commands = uploadedCode.Split('\n');
            foreach (var line in commands)
            {
                var splitedLine = line.Split(' ');
                if (line.StartsWith(" "))
                    _functions[lastDefinedFunc] += line.Substring(4) + '\n';
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
                            _functions.Add(splitedLine[1], "");
                            lastDefinedFunc = splitedLine[1];
                            break;
                        case "call":
                            ExecuteUploadedCode(_functions[splitedLine[1]]);
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