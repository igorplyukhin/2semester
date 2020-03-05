using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        private static Dictionary<int, int> brackets;
        private static Stack<int> openBrackets;

        private static void FindBrackets(IVirtualMachine vm)
        {
            brackets = new Dictionary<int, int>();
            openBrackets = new Stack<int>();
            for (var i = 0; i < vm.Instructions.Length; i++)
                switch (vm.Instructions[i])
                {
                    case '[':
                        openBrackets.Push(i);
                        break;
                    case ']':
                        brackets.Add(openBrackets.Peek(), i);
                        brackets.Add(i, openBrackets.Pop());
                        break;
                }
        }
        

        public static void RegisterTo(IVirtualMachine vm)
        {
            FindBrackets(vm);
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
    }
}