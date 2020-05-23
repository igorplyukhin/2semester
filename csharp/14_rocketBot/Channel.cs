using System;
using System.Collections.Generic;


namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        private List<T> container = new List<T>();
        private T lastItem;

        public T this[int index]
        {
            get
            {
                lock (container)
                    if (index >= 0 && index < container.Count)
                        return container[index];
                    else
                        return null;
            }
            set
            {
                lock (container)
                {
                    if (index < 0 || index > container.Count)
                        throw new IndexOutOfRangeException();
                    
                    if (index == container.Count - 1 || index == container.Count)
                        lastItem = value;
                    
                    if (index == container.Count)
                        container.Add(value);
                    else
                    {
                        container[index] = value;
                        var rangeToRemoveAfterChangedItem = container.Count - 1 - index;
                        container.RemoveRange(index+1, rangeToRemoveAfterChangedItem);
                    }
                }
            }
        }
        
        public T LastItem()
        {
            lock (container)
                return lastItem;
        }
        
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (container)
                if (knownLastItem == lastItem)
                {
                    lastItem = item;
                    container.Add(item);
                }
        }
        
        public int Count
        {
            get
            {
                lock (container)
                    return container.Count;
            }
        }
    }
}