using System.Collections.Generic;

namespace TodoApplication
{
    public class ListModel<TItem>
    {
        private readonly LimitedSizeStack<Command> backup;
        public List<TItem> Items { get; }

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            backup = new LimitedSizeStack<Command>(limit);
        }
        
        private abstract class Command
        {
            public abstract void Execute();
            public abstract void Undo();
        }
        
        private class AddCommand : Command
        {
            private readonly List<TItem> items;
            private readonly TItem item;
            private readonly LimitedSizeStack<Command> backup;

            public AddCommand(List<TItem> items, TItem item, LimitedSizeStack<Command> backup)
            {
                this.items = items;
                this.item = item;
                this.backup = backup;
            }

            public override void Execute()
            {
                items.Add(item);
                backup.Push(this);
            }

            public override void Undo() => items.RemoveAt(items.Count - 1);
        }
        
        private class RemoveCommand : Command
        {
            private readonly List<TItem> items;
            private readonly LimitedSizeStack<Command> backup;
            private readonly int index;
            private TItem removedItem;

            public RemoveCommand(List<TItem> items, LimitedSizeStack<Command> backup, int index)
            {
                this.items = items;
                this.backup = backup;
                this.index = index;
            }

            public override void Execute()
            {
                removedItem = items[index];
                items.RemoveAt(index);
                backup.Push(this);
            }

            public override void Undo() => items.Insert(index, removedItem);
        }
        
        public void AddItem(TItem item) => new AddCommand(Items, item, backup).Execute();

        public void RemoveItem(int index) => new RemoveCommand(Items, backup, index).Execute();

        public bool CanUndo() => backup.Count > 0;

        public void Undo() => backup.Pop().Undo();
    }
}