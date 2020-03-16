using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		readonly Dictionary<char, Action<IVirtualMachine>> commands;
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }
		
		public VirtualMachine(string program, int memorySize)
		{
			Instructions = program;
			Memory = new byte[memorySize];
			MemoryPointer = 0;
			commands = new Dictionary<char, Action<IVirtualMachine>>();
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute) => commands.Add(symbol, execute);

		public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				var command = Instructions[InstructionPointer];
				if (commands.TryGetValue(command, out var value))
					commands[command](this);
				InstructionPointer++;
			}
		}
	}
}