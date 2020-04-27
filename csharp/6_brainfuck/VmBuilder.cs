using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VmBuilder
    {
        private List<Action<VirtualMachine>> modifications = new List<Action<VirtualMachine>>();
        private int memSize;
        private VirtualMachine vm;

        public VmBuilder(int memSize)
        {
            this.memSize = memSize;
        }

        public VmBuilder AddBasicCommands(Func<int> read, Action<char> write)
        {
            modifications.Add(delegate { BrainfuckBasicCommands.RegisterTo(vm, read, write); });
            return this;
        }

        public VmBuilder AddLoopCommands()
        {
            modifications.Add(delegate { BrainfuckLoopCommands.RegisterTo(vm); });
            return this;
        }

        public void Init(VirtualMachine vm)
        {
            foreach (var e in modifications)
                e(vm);
        }

        public VirtualMachine Build(string program)
        {
            var vm = new VirtualMachine(program, memSize);
            this.vm = vm;
            Init(vm);
            return vm;
        }
    }

    public class Program1
    {
        public static void Main1()
        {
            var vmBuilder = new VmBuilder(memSize: 60)
                .AddBasicCommands(() => Console.Read(), c => Console.Write(c))
                .AddLoopCommands();
            vmBuilder.Build(Program.sierpinskiTriangleBrainfuckProgram).Run(); // Build(...) возвращает созданную Vm
            vmBuilder.Build(@"
                 +++++++++++++++++++++++++++++++++++++++++++++
                 +++++++++++++++++++++++++++.+++++++++++++++++
                 ++++++++++++.+++++++..+++.-------------------
                 ---------------------------------------------
                 ---------------.+++++++++++++++++++++++++++++
                 ++++++++++++++++++++++++++.++++++++++++++++++
                 ++++++.+++.------.--------.------------------
                 ---------------------------------------------
                 ----.-----------------------.
                ").Run();
        }
    }
}