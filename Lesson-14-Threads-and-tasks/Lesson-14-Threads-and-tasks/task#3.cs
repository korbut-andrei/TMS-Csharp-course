using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

// Создать консольное приложение, которое в реальном времени будет
//обрабатывать очень длинный список чисел, находя числа, удовлетворяющие
//некоторому условию (например, числа делящиеся на 3, или простые числа, и
//т.д.). Обработку списка производить в нескольких потоках, каждому потоку
//выделить свой диапазон. Найденные числа выводить в реальном времени в таблицу.

int[] numbers = new int[1000];
Random random = new Random();
for (int i = 0; i < numbers.Length; i++)
{
    numbers[i] = random.Next(-10_000, 10_001);
}

do
{
    Console.WriteLine("Выбери номер операции:" +
    "\n1. Найти числа кратные 3" +
    "\n2. Найти простые числа");
    var userAnswer = Console.ReadLine();

    switch (userAnswer)
    {
        case "1":
            {
                int numThreads = 4;

                Task<List<int>>[] tasks = new Task<List<int>>[numThreads];

                int chunkSize = numbers.Length / numThreads;

                for (int i = 0; i < numThreads; i++)
                {
                    int startIndex = i * chunkSize;
                    int endIndex = (i == numThreads - 1) ? numbers.Length : (i + 1) * chunkSize;

                    tasks[i] = Task.Factory.StartNew(() =>
                    {
                        Console.WriteLine($"Task {i + 1} started");

                        List<int> result = AliquoteOf3(numbers, startIndex, endIndex, i);

                        Console.WriteLine($"Task {i + 1} completed");

                        return result;
                    });
                }


                Task.WaitAll(tasks);

                Console.WriteLine($"Calculations completed");

            }
            break;

        case "2":
            {
                int numThreads = 4;

                Task<List<int>>[] tasks = new Task<List<int>>[numThreads];

                int chunkSize = numbers.Length / numThreads;

                long totalSum = 0;

                for (int i = 0; i < numThreads; i++)
                {
                    int startIndex = i * chunkSize;
                    int endIndex = (i == numThreads - 1) ? numbers.Length : (i + 1) * chunkSize;

                    tasks[i] = Task.Factory.StartNew(() =>
                    {
                        Console.WriteLine($"Task {i + 1} started");

                        List<int> primeNumbers = FindPrimes(numbers, startIndex, endIndex, i);

                        Console.WriteLine($"Task {i + 1} completed");

                        return primeNumbers;
                    });
                }

                Task.WaitAll(tasks);

                Console.WriteLine($"Calculations completed");
            }
            break;

        default:

            break;
    }
}
while (true);

static List<int> AliquoteOf3(int[] numbers, int startIndex, int endIndex, int taskNumber)
{
    List<int> intsAliquoteOf3 = new List<int>();
    for (int i = startIndex; i < endIndex; i++)
    {
        if (numbers[i] % 3 == 0)
        {
            Console.WriteLine($"Task #{taskNumber}: {numbers[i]} can be divided by 3");
            intsAliquoteOf3.Add(numbers[i]);
        }
    }
    return intsAliquoteOf3;
}


static List<int> FindPrimes(int[] numbers, int startIndex, int endIndex, int taskNumber)
{
    List<int> primes = new List<int>();

    for (int i = startIndex; i < endIndex; i++)
    {
        if (IsPrime(numbers[i]))
        {
            Console.WriteLine($"Task #{taskNumber}: {numbers[i]} is prime");
            primes.Add(numbers[i]);
        }
    }

    return primes;
}

static bool IsPrime(int number)
{
    if (number < 2)
    {
        return false;
    }

    for (int i = 2; i <= Math.Sqrt(number); i++)
    {
        if (number % i == 0)
        {
            return false;
        }
    }
    return true;
}