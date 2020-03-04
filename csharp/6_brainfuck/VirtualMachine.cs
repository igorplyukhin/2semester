using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public VirtualMachine(string program, int memorySize)
		{
			Instructions = program;
			Memory = new byte[memorySize];
			MemoryPointer = 0;
			commands = new Dictionary<char, Action<IVirtualMachine>>();
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute) => commands.Add(symbol, execute);

		private Dictionary<char, Action<IVirtualMachine>> commands { get; }
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }

		public int MemoryPointer { get; set; }

		public void Run()
		{
			for (InstructionPointer = InstructionPointer; InstructionPointer < Instructions.Length; InstructionPointer++)
			{
				var command = Instructions[InstructionPointer];
				if (!commands.ContainsKey(command))
					continue;
				commands[command](this);
			}
		}
	}
}