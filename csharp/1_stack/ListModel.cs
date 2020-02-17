using System.Collections.Generic;

namespace TodoApplication
{
    public class ListModel<TItem>
    {
        private readonly LimitedSizeStack<ICommand> backup;
        public List<TItem> Items { get; }

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            backup = new LimitedSizeStack<ICommand>(limit);
        }

        public void AddItem(TItem item) => new AddCommand<TItem>(Items, item, backup).Execute();

        public void RemoveItem(int index) => new RemoveCommand<TItem>(Items, backup, index).Execute();

        public bool CanUndo() => backup.Count > 0;

        public void Undo() => backup.Pop().Undo();
    }
    
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
        
    public class AddCommand<TItem> : ICommand
    {
        private readonly List<TItem> items;
        private readonly TItem item;
        private readonly LimitedSizeStack<ICommand> backup;

        public AddCommand(List<TItem> items, TItem item, LimitedSizeStack<ICommand> backup)
        {
            this.items = items;
            this.item = item;
            this.backup = backup;
        }

        public void Execute()
        {
            items.Add(item);
            backup.Push(this);
        }

        public void Undo() => items.RemoveAt(items.Count - 1);
    }
        
    public class RemoveCommand<TItem> : ICommand
    {
        private readonly List<TItem> items;
        private readonly LimitedSizeStack<ICommand> backup;
        private readonly int index;
        private TItem removedItem;

        public RemoveCommand(List<TItem> items, LimitedSizeStack<ICommand> backup, int index)
        {
            this.items = items;
            this.backup = backup;
            this.index = index;
        }

        public void Execute()
        {
            removedItem = items[index];
            items.RemoveAt(index);
            backup.Push(this);
        }

        public void Undo() => items.Insert(index, removedItem);
    }
}