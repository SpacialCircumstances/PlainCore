using System;
using System.Collections;
using System.Collections.Generic;

namespace PlainCore
{
    public class ArrayBufferList<T> : IList<T>
    {
        private const string NOT_SUPPORTED_MESSAGE = "";

        public ArrayBufferList(int capacity = 64)
        {
            array = new T[capacity];
            Count = 0;
        }

        private T[] array;
        public T[] Buffer => array;
        public int Capacity => array.Length;
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
                return array[index];
            }
            set
            {
                if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
                array[index] = value;
            }
        }

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        protected void EnsureCapacity()
        {
            var len = array.Length;
            if (Count >= len)
            {
                int newLen = len * 2;
                Array.Resize(ref array, newLen);
            }
        }

        public void Add(T item)
        {
            EnsureCapacity();
            array[Count] = item;
            Count++;
        }

        public void Clear()
        {
            Count = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (item.Equals(array[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this.array, 0, array, arrayIndex, Count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(Count, array.GetEnumerator());
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (item.Equals(array[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            //Should not be used. This is not a normal list.
            throw new NotSupportedException(NOT_SUPPORTED_MESSAGE);
        }

        public bool Remove(T item)
        {
            //Should not be used. This is not a normal list.
            throw new NotSupportedException(NOT_SUPPORTED_MESSAGE);
        }

        public void RemoveAt(int index)
        {
            //Should not be used. This is not a normal list.
            throw new NotSupportedException(NOT_SUPPORTED_MESSAGE);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected class Enumerator : IEnumerator<T>
        {
            public Enumerator(int max, IEnumerator arrayEnumerator)
            {
                this.max = max;
                this.arrayEnumerator = arrayEnumerator;
            }

            private readonly int max;
            private int index = 0;
            private readonly IEnumerator arrayEnumerator;

            public T Current => (T)arrayEnumerator.Current;

            object IEnumerator.Current => arrayEnumerator.Current;

            public bool MoveNext()
            {
                if (index < max)
                {
                    index++;
                    return arrayEnumerator.MoveNext();
                } else
                {
                    return false;
                }
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public void Dispose()
            {
                
            }
        }
    }
}
