using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        private static Dictionary<string, string> constants = new Dictionary<string, string>
        {
            {"lowerAlph", "abcdefghijklmnopqrstuvwxyz"},
            {"upperAlph", "ABCDEFGHIJKLMNOPQRSTUVWXYZ"},
            {"digits", "0123456789"}
        };

        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', b => write((char) vm.Memory[vm.MemoryPointer]));
            vm.RegisterCommand('+', b => vm.Memory[vm.MemoryPointer]++);
            vm.RegisterCommand('-', b => vm.Memory[vm.MemoryPointer]--);
            vm.RegisterCommand(',', b => vm.Memory[vm.MemoryPointer] = (byte) read());
            vm.RegisterCommand('>', b => vm.MemoryPointer++);
            vm.RegisterCommand('<', b => vm.MemoryPointer--);
            foreach (var ch in constants.SelectMany(e => e.Value))
                vm.RegisterCommand(ch, b => vm.Memory[vm.MemoryPointer] = (byte) ch);
        }
    }
}