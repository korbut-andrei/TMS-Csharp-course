using System;
using System.Collections;
using System.Collections.Generic;

class Program
{
    public class MyCollection<T> : IEnumerable<T>
    {
        private T[] elements;

        public MyCollection(params T[] elements)
        {
            this.elements = elements;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MyEnumerator(this.elements);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class MyEnumerator : IEnumerator<T>
        {
            private readonly T[] elements;
            private int currentIndex;

            public MyEnumerator(T[] elements)
            {
                this.elements = elements;
                currentIndex = -1; // Start before the first element
            }

            public T Current => currentIndex < elements.Length - 1 ? 
                Add(elements[currentIndex], elements[currentIndex + 1]) 
                : Add(elements[currentIndex], elements[currentIndex]);

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                currentIndex++;
                return currentIndex < elements.Length;
            }

            public void Reset()
            {
                currentIndex = -1;
            }

            public void Dispose()
            {
                // Dispose if necessary
            }

            private T Add(T a, T b)
            {
                dynamic x = a;
                dynamic y = b;
                return x + y;
            }
        }
    }

    static void Main(string[] args)
    {
        MyCollection<int> collection = new MyCollection<int>(1, 2, 3, 4, 5);

        foreach (int item in collection)
        {
            Console.WriteLine(item);
        }

        Console.ReadLine();
    }
}
