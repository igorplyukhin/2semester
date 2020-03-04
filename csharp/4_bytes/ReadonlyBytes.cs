using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable
    {
        private readonly byte[] bytes;
        private readonly int hash;
        public int Length => bytes.Length;

        private int CalcHash()
        {
            unchecked
            {
                ulong hash = 14695981039346656037;
                ulong prime = 1099511628211;
                for (var i = 0; i < Length; i++)
                {
                    hash *= prime;
                    hash = hash ^ bytes[i];
                }

                return (int) hash;
            }
        }


        public ReadonlyBytes(params byte[] items)
        {
            if (items == null)
                throw new ArgumentNullException();
            bytes = items;
            hash = CalcHash();
        }

        public byte this[int index] => bytes[index];

        public IEnumerator<byte> GetEnumerator()
        {
            foreach (var e in bytes)
                yield return e;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ReadonlyBytes)) return false;
            var secondBytes = (ReadonlyBytes) obj;
            if (bytes.Length != secondBytes.Length)
                return false;
            for (var i = 0; i < Length; i++)
            {
                if (bytes[i] != secondBytes[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode() => hash;

        public override string ToString() => $"[{string.Join(", ", bytes)}]";
    }
}