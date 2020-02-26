using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable
    {
        private readonly byte[] _bytes;
        public readonly int Hash;
        public int Length => _bytes.Length;
        

        public ReadonlyBytes(params byte[] items)
        {
            if (items == null)
                throw new ArgumentNullException();
            _bytes = items;
            
            unchecked
            {
                var p = 9973;
                var hash = 216613;
                foreach (var e in _bytes)
                {
                    hash = (hash * p + e) % 32768;
                }

                Hash = hash;
            }
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index > Length) throw new IndexOutOfRangeException();
                return _bytes[index];
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            foreach (var e in _bytes)
            {
                yield return e;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ReadonlyBytes)) return false;
            var secondBytes = (ReadonlyBytes) obj;
            if (_bytes.Length != secondBytes.Length)
                return false;
            for (var i = 0; i < Length; i++)
            {
                if (_bytes[i] != secondBytes[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Hash;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Length == 0)
                return "[]";
            
            if (Length == 1)
                return $"[{_bytes[0]}]";
            
            for (var i = 0; i < Length; i++)
            {
                if (i == 0)
                    sb.Append($"[{_bytes[i]},");
                else if (i == Length - 1)
                    sb.Append($" {_bytes[i]}]");
                else 
                    sb.Append($" {_bytes[i]},");
            }

            return sb.ToString();
        }
    }
}