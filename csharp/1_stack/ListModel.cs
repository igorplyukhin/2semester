using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public class Command<TItem>
    {
        public TItem Item;

        public Command(TItem item)
        {
            Item = item;
        }
    }

    class AddItem<TItem>:Command<TItem>
    {
        public AddItem(TItem item) : base(item)
        {
        }
    }

    class RemoveItem<TItem>:Command<TItem>
    {
        public int Index;
        public RemoveItem(TItem item, int index) : base(item)
        {
            Index = index;
        }
    }
    
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        public int Limit;
        public LimitedSizeStack<Command<TItem>> Backup;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
            Backup = new LimitedSizeStack<Command<TItem>>(limit);
        }

        public void AddItem(TItem item)
        {
            Items.Add(item);
            Backup.Push(new AddItem<TItem>(item));
        }

        public void RemoveItem(int index)
        {
            var element = Items[index];
            Items.RemoveAt(index);
            Backup.Push(new RemoveItem<TItem>(element, index));
        }

        public bool CanUndo()
        {
            return Backup.Count > 0;
        }

        public void Undo()
        {
            var lastCommand = Backup.Pop();
            if (lastCommand is AddItem<TItem>)
            {
                Items.RemoveAt(Items.Count - 1);
            }
            else if (lastCommand is RemoveItem<TItem>)
            {
                var removeCommand = (RemoveItem<TItem>) lastCommand;
                Items.Insert(removeCommand.Index, removeCommand.Item);
            }
            else
            {
                throw new Exception("Unknown command");
            }
        }
    }
}
