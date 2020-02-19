using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring
{
    public class RemoteController
    {
        private readonly Dictionary<string, int?> currentOptions = new Dictionary<string, int?>
            {{"brightness", 50}, {"contrast", 50}, {"volume", 30}};

        private bool isOnline;
        

        public string Call(string command)
        {
            var subCommands = "";
            if (command.StartsWith("Options change"))
            {
                subCommands = command.Substring(14).Trim();
                command = "Options change";
            }

            var possibleCommands = new Dictionary<string, Action>
            {
                {"Tv On", delegate { isOnline = true; }},
                {"Tv Off", delegate { isOnline = false; }},
                {"Volume Up", delegate { currentOptions["volume"] += 10; }},
                {"Volume Down", delegate { currentOptions["volume"] -= 10; }},
                {"Options change", delegate { ChangeOptions(subCommands); }},
            };

            if (command == "Options show")
            {
                var optionsShower = new OptionsShower(currentOptions, isOnline);
                return optionsShower.ShowOptions();
            }

            possibleCommands[command]();
            return null;
        }

        private void ChangeOptions(string commands)
        {
            var operations = new Dictionary<string, Func<int?, int?>> {{"up", x => x}, {"down", x => -x}};
            var splitedCommands = commands.Split();
            for (var i = 0; i < splitedCommands.Length - 2; i += 2)
                currentOptions[splitedCommands[i]] += operations[splitedCommands[i + 1]](10);
        }
    }

    public class OptionsShower
    {
        private readonly Dictionary<string, int?> _currentOptions;
        private readonly bool _isOnline;

        public OptionsShower(Dictionary<string, int?> currentOptions, bool isOnline)
        {
            _currentOptions = currentOptions;
            _isOnline = isOnline;
        }

        public string ShowOptions()
        {
            var output = new StringBuilder();
            output.AppendLine("Options:");
            output.AppendLine($"Volume {_currentOptions["volume"]}");
            output.AppendLine($"IsOnline {_isOnline}");
            output.AppendLine($"Brightness {_currentOptions["brightness"]}");
            output.AppendLine($"Contrast {_currentOptions["contrast"]}");
            return output.ToString();
        }
    }
}