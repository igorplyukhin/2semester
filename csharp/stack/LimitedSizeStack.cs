using System;

namespace TodoApplication
{
    public class StackItem<T>
    {
        public T Value { get; set; }
        public StackItem<T> Next { get; set; }
        public StackItem<T> Prev { get; set; }
    }

    public class LimitedSizeStack<T>
    {
        private StackItem<T> head;
        private StackItem<T> tail;
        private readonly int lim;
        private int count;

        public LimitedSizeStack(int limit) => lim = limit;

        public void Push(T item)
        {
            if (head == null)
                tail = head = new StackItem<T> {Value = item, Next = null, Prev = null};
            else
            {
                var lastItem = new StackItem<T> {Value = item, Next = null, Prev = tail};
                tail.Next = lastItem;
                tail = lastItem;
            }

            count++;
            if (count > lim)
            {
                head = head.Next;
                head.Prev = null;
                count--;
            }
        }

        public T Pop()
        {
            if (tail == null) throw new InvalidOperationException();
            var result = tail.Value;
            tail = tail.Prev;
            if (tail == null)
                head = null;
            count--;
            return result;
        }

        public int Count => count;
    }
}