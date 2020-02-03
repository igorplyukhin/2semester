using System;
using System.Collections.Generic;

namespace Clones
{
	public class StackItem<T>
	{
		public T Value { get; set; }
		public StackItem<T> Prev { get; set; }
	}

	public class LinkedListStack<T> : ICloneable
	{
		private StackItem<T> head;
		private StackItem<T> tail;
		public int Count { get; private set; }

		public LinkedListStack(StackItem<T> element)
		{
			head = element;
		}
		
		public LinkedListStack(T element)
		{
			head = new StackItem<T> {Value = element, Prev = null};
		}
		
		public void Push(T item)
		{
			if (head == null)
				tail = head = new StackItem<T> {Value = item, Prev = null};
			else
			{
				var lastItem = new StackItem<T> {Value = item, Prev = tail};
				tail = lastItem;
			}

			Count++;
		}

		public T Pop()
		{
			if (tail == null) throw new InvalidOperationException();
			var result = tail.Value;
			tail = tail.Prev;
			
			if (tail == null)
				head = null;
			Count--;
			return result;
		}

		public T Last()
		{
			if (tail == null) throw new InvalidOperationException();
			return tail.Value;
		}

		public object Clone()
		{
			return new LinkedListStack<T>(tail);
		}
	}
	
	public class CloneVersionSystem : ICloneVersionSystem
	{
		private List<LinkedListStack<int>> LearnedPrograms = 
			new List<LinkedListStack<int>>{new LinkedListStack<int>(1)};
		private List<LinkedListStack<int>> RolledbackPrograms =
			new List<LinkedListStack<int>>();

		public string Learn(int cloneId, int programId)
		{
			LearnedPrograms[cloneId].Push(programId);
			return null;
		}
		
		public string Execute(string query)
		{
			var s = query.Split(' ');
			if (s[0] == "learn")
			{
				Learn(int.Parse(s[1]) - 1, int.Parse(s[2]));
			}

			if (s[0] == "check")
			{
				var cloneId = int.Parse(s[1]) -1 ;
				if (LearnedPrograms[int.Parse(s[1]) - 1].Count > 0)
					return LearnedPrograms[int.Parse(s[1]) - 1].Last().ToString();
				return "basic";
			}

			if (s[0] == "rollback")
			{
				var cloneId = int.Parse(s[1]) - 1;
				var p = LearnedPrograms[cloneId].Pop();
				RolledbackPrograms.Add(new LinkedListStack<int>(p));
			}
			
			return null;
		}
	}
}
