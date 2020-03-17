using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            var brackets = FindBrackets(vm.Instructions);
            vm.RegisterCommand('[', b =>
            {
                if (vm.Memory[vm.MemoryPointer] == 0)
                    vm.InstructionPointer = brackets[vm.InstructionPointer];
            });
            vm.RegisterCommand(']', b =>
            {
                if (vm.Memory[vm.MemoryPointer] != 0)
                    vm.InstructionPointer = brackets[vm.InstructionPointer];
            });
        }

        private static Dictionary<int, int> FindBrackets(string instructions)
        {
            var brackets = new Dictionary<int, int>();
            var openBrackets = new Stack<int>();
            for (var i = 0; i < instructions.Length; i++)
                switch (instructions[i])
                {
                    case '[':
                        openBrackets.Push(i);
                        break;
                    case ']':
                        brackets.Add(openBrackets.Peek(), i);
                        brackets.Add(i, openBrackets.Pop());
                        break;
                }

            return brackets;
        }
    }
}