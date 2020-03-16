using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        static readonly List<Tuple<char, char>> bounds = new List<Tuple<char, char>>
            {Tuple.Create('a', 'z'), Tuple.Create('A', 'Z'), Tuple.Create('0', '9')};

        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', b => write((char) vm.Memory[vm.MemoryPointer]));
            vm.RegisterCommand(',', b => vm.Memory[vm.MemoryPointer] = (byte) read());
            vm.RegisterCommand('+', b =>
                vm.Memory[vm.MemoryPointer] = (byte) AddByModule(vm.Memory[vm.MemoryPointer], 1, 256));
            vm.RegisterCommand('-', b =>
                vm.Memory[vm.MemoryPointer] = (byte) SubtractByModule(vm.Memory[vm.MemoryPointer], 1, 256));
            vm.RegisterCommand('>', b =>
                vm.MemoryPointer = AddByModule(vm.MemoryPointer, 1, vm.Memory.Length));
            vm.RegisterCommand('<', b =>
                vm.MemoryPointer = SubtractByModule(vm.MemoryPointer, 1, vm.Memory.Length));
            RegisterAlphAndDigits(vm);
        }
        
        private static int AddByModule(int a, int b, int module) => (a + b) % module;

        private static int SubtractByModule(int a, int b, int module) => (module + a - b) % module;

        private static void RegisterAlphAndDigits(IVirtualMachine vm)
        {
            foreach (var (start, end) in bounds)
                for (var i = start; i <= end; i++)
                {
                    var ch = i;
                    vm.RegisterCommand(ch, b => vm.Memory[vm.MemoryPointer] = (byte) ch);
                }
        }
    }
}