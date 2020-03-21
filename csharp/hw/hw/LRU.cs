using System.Collections.Generic;

namespace hw
{
    public class LRU<TKey, TValue>
    {
        public int Count => container.Count;
        private Dictionary<TKey, TValue> container = new Dictionary<TKey, TValue>();
        private LinkedList<TKey> llist = new LinkedList<TKey>();

        public void Add(TKey key, TValue value)
        {
            container.Add(key,value);
            llist.AddLast(key);
        }

        public TValue Get(TKey key)
        {
            if (!container.ContainsKey(key))
                throw new KeyNotFoundException();
            llist.Remove(key);
            llist.AddLast(key);
            return container[key];
        }

        public void RemoveLastRecentlyUsed()
        {
            var key = llist.First.Value;
            container.Remove(key);
            llist.RemoveFirst();
        }
    }
}