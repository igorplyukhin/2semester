using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable
    {
        private readonly byte[] bytes;
        private readonly int hash;
        public int Length => bytes.Length;


        public ReadonlyBytes(params byte[] items)
        {
            if (items == null)
                throw new ArgumentNullException();
            bytes = items;
            unchecked
            {
                ulong hash = 14695981039346656037;
                ulong prime = 1099511628211;
                for (var i = 0; i < Length; i++)
                {
                    hash *= prime;
                    hash = hash ^ bytes[i];
                }

                this.hash = (int)hash;
            }
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index > Length) throw new IndexOutOfRangeException();
                return bytes[index];
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            foreach (var e in bytes)
                yield return e;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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

        public override int GetHashCode()
        {
            return hash;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Length == 0)
                return "[]";

            if (Length == 1)
                return $"[{bytes[0]}]";

            for (var i = 0; i < Length; i++)
            {
                if (i == 0)
                    sb.Append($"[{bytes[i]},");
                else if (i == Length - 1)
                    sb.Append($" {bytes[i]}]");
                else
                    sb.Append($" {bytes[i]},");
            }

            return sb.ToString();
        }
    }
}