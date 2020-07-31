using System;

namespace Entygine
{
    public class StructArray<T0> where T0 : struct
    {
        private int size;
        private int capacity;
        private T0[] array;

        private const int MIN_CAPACITY = 4;

        public StructArray()
        {
            size = 0;
            array = new T0[0];
        }
        public StructArray(int capacity, bool fill)
        {
            size = fill ? capacity : 0;
            this.capacity = capacity;
            array = new T0[capacity];
        }

        public ref T0 this[int index]
        {
            get
            {
                if (index >= size)
                    throw new IndexOutOfRangeException();

                return ref array[index];
            }
        }

        public void Add(T0 value)
        {
            if (size >= capacity)
            {
                int newCapacity = capacity * 2;
                if (newCapacity < MIN_CAPACITY)
                    newCapacity = MIN_CAPACITY;

                ResizeCapacity(newCapacity);
            }

            array[size] = value;
            size++;
        }

        public void Insert(int index, T0 value)
        {
            if (index > size)
                throw new IndexOutOfRangeException();

            if (size >= capacity)
            {
                int newCapacity = capacity * 2;
                if (newCapacity < MIN_CAPACITY)
                    newCapacity = MIN_CAPACITY;

                ResizeCapacity(newCapacity);
            }

            for (int i = size; i > index; i--)
                array[i] = array[i - 1];

            array[index] = value;
            size++;
        }

        public void SwapForLast(T0 value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(value))
                {
                    array[i] = array[size - 1];
                    size--;
                    return;
                }
            }
        }
        public void SwapForLast(int index)
        {
            if (index >= size)
                throw new IndexOutOfRangeException();

            array[index] = array[size - 1];
            size--;
        }

        private void ResizeCapacity(int capacity)
        {
            Array.Resize(ref array, capacity);
            this.capacity = capacity;
        }

        public int Count => size;
    }
}
