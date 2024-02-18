using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_Generics
{
    public class ComparablePair<T, U> : IComparable<ComparablePair<T, U>>
      where T : IComparable<T>
      where U : IComparable<U>
    {
        public T First { get; set; }
        public U Second { get; set; }

        public ComparablePair(T first, U second)
        {
            First = first;
            Second = second;
        }

        public int CompareTo(ComparablePair<T, U> other)
        {
            int compareFirst = First.CompareTo(other.First);

            return compareFirst != 0 ? compareFirst : Second.CompareTo(other.Second);
        }

        public void WriteConclusion(int result)
        {
            if (result < 0)
                Console.WriteLine("pair1 is less than pair2");
            else if (result > 0)
                Console.WriteLine("pair1 is greater than pair2");
            else
                Console.WriteLine("pair1 is equal to pair2");
        }
    }

    class Program
    {
        static void Main()
        {
            ComparablePair<int, string> pair1 = new ComparablePair<int, string>(10, "apple");
            ComparablePair<int, string> pair2 = new ComparablePair<int, string>(10, "orange");

            int result = pair1.CompareTo(pair2);
            pair1.WriteConclusion(result);

            ComparablePair<int, int> pair3 = new ComparablePair<int, int>(55, 10);
            ComparablePair<int, int> pair4 = new ComparablePair<int, int>(55, 2);

            int result2 = pair3.CompareTo(pair4);
            pair3.WriteConclusion(result2);

            Console.ReadLine();
        }
    }
}



class numbers
{
    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < 10; i++) 
        {
        yield return i;
        }
    }
}   