using System;
using System.Collections.Generic;

namespace Clones
{
    public class StackItem<T>
    {
        public T Value { get; set; }
        public StackItem<T> Prev { get; set; }
    }

    public class LinkedListStack<T>
    {
        private StackItem<T> head;
        private StackItem<T> tail;
        public int Count { get; private set; }
        

        public LinkedListStack(StackItem<T> element) => tail = head = element;
        
        public LinkedListStack(T element) => tail = head = new StackItem<T> {Value = element, Prev = null};
        
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

        public LinkedListStack<T> Clone() => new LinkedListStack<T>(tail) {Count = Count};
    }

    public class CloneVersionSystem : ICloneVersionSystem
    {
        private readonly List<LinkedListStack<int>> clonesLearnedPrograms =
            new List<LinkedListStack<int>> {new LinkedListStack<int>(1)};

        private readonly List<LinkedListStack<int>> clonesRolledbackPrograms =
            new List<LinkedListStack<int>> {new LinkedListStack<int>(null)};

        private string Learn(int cloneId, int programId)
        {
            clonesLearnedPrograms[cloneId].Push(programId);
            return null;
        }

        private string Rollback(int cloneId)
        {
            var programToRollback = clonesLearnedPrograms[cloneId].Pop();
            clonesRolledbackPrograms[cloneId].Push(programToRollback);
            return null;
        }

        private string Relearn(int cloneId)
        {
            var programToRelearn = clonesRolledbackPrograms[cloneId].Pop();
            Learn(cloneId, programToRelearn);
            return null;
        }

        private string Check(int cloneId)
        {
            return
                clonesLearnedPrograms[cloneId].Count > 0
                    ? clonesLearnedPrograms[cloneId].Last().ToString()
                    : "basic";
        }

        private string Clone(int cloneId)
        {
            clonesLearnedPrograms.Add(clonesLearnedPrograms[cloneId].Clone());
            clonesRolledbackPrograms.Add(clonesRolledbackPrograms[cloneId].Clone());
            return null;
        }


        public string Execute(string query)
        {
            var cloneFunctions = new Dictionary<string, Func<int, string>>
            {
                {"check", Check},
                {"rollback", Rollback},
                {"relearn", Relearn},
                {"clone", Clone}
            };
            var splitedQuery = query.Split(' ');
            var cloneId = int.Parse(splitedQuery[1]) - 1;

            return
                splitedQuery[0] == "learn"
                    ? Learn(cloneId, int.Parse(splitedQuery[2]))
                    : cloneFunctions[splitedQuery[0]](cloneId);
        }
    }
}