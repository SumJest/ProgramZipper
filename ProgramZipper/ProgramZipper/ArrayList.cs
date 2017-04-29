using System;

namespace sumjest
{
    public class ArrayList<T>
    {
        T[] massive;
        int length = 0;
        public int Length { get { return length; } }

        public ArrayList()
        {
            massive = new T[256];
        }

        public ArrayList(int maxlength)
        {
            massive = new T[maxlength];
        }
        public void Clear()
        {
            length = 0;
        }
        public void Add(T value)
        {
            if (length > massive.Length) throw new arrayListTooBigException("Array list should be no more than max value, default: 256");
            massive[length] = value;
            length++;
        }
             
        public T[] toArray()
        {
            T[] m = new T[length];
            for (int i = 0; i < length; i++) m[i] = massive[i];
            return m;
        }
        public void RemoveLast() { if (length == 0) { throw new arrayListException("ArrayList is empty"); } length--; }

        public T Get(int index)
        {
            if (index >= length) throw new arrayListInvalidIndexException("Invalid index");
            return massive[index];
        }
        public void RemoveValue(T value)
        {
            for (int i = 0; i < length; i++)
            {
                if (massive[i].Equals(value))
                {
                    
                    for (int i1 = i; i1 < length - 1; i1++) { Swap(massive, i1, i1 + 1); }
                    break;
                }

            }
            
            if (!massive[length - 1].Equals(value)) { throw new arrayListException("Invalid value"); }
            RemoveLast();
        }

        public bool Contains(T value)
        {
            for (int i = 0; i < length; i++) { if (massive[i].Equals(value)) return true; }
            return false;
        }


        private void Swap(T[] array, int index1, int index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }

    }
    class arrayListTooBigException : Exception { public arrayListTooBigException(string message) : base(message) { } }
    class arrayListInvalidIndexException : Exception { public arrayListInvalidIndexException(string message) : base(message) { } }
    class arrayListException : Exception { public arrayListException(string message) : base(message) { } }
}

