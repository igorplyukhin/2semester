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
            vm.RegisterCommand('.', b =>
                write((char) vm.Memory[vm.MemoryPointer]));
            vm.RegisterCommand('+', b =>
                vm.Memory[vm.MemoryPointer] = (byte) ((vm.Memory[vm.MemoryPointer] + 1) % 256));
            vm.RegisterCommand('-', b =>
                vm.Memory[vm.MemoryPointer] = (byte) ((256 + vm.Memory[vm.MemoryPointer] - 1) % 256));
            vm.RegisterCommand(',', b =>
                vm.Memory[vm.MemoryPointer] = (byte) read());
            vm.RegisterCommand('>', b =>
                vm.MemoryPointer = (vm.MemoryPointer + 1) % vm.Memory.Length);
            vm.RegisterCommand('<', b =>
                vm.MemoryPointer = (vm.Memory.Length + vm.MemoryPointer - 1) % vm.Memory.Length);
            foreach (var ch in constants.SelectMany(e => e.Value))
                vm.RegisterCommand(ch, b => vm.Memory[vm.MemoryPointer] = (byte) ch);
        }
    }
}