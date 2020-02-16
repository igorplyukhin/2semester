using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        private readonly int lim;
        private LinkedList<T> stack = new LinkedList<T>();
        private int count;

        public LimitedSizeStack(int limit) => lim = limit;

        public void Push(T item)
        {
            stack.AddLast(new LinkedListNode<T>(item));

            count++;
            if (count > lim)
            {
               stack.RemoveFirst();
               count--;
            }
        }

        public T Pop()
        {
            var lastElement = stack.Last.Value;
            stack.RemoveLast();
            count--;
            return lastElement;
        }

        public int Count => count;
    }
}