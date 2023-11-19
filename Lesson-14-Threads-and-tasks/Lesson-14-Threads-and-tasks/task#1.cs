using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

/*List <int> numbersList = new List<int>();
int[] numbersArray = new int[1];
Stopwatch stopwatch = new Stopwatch();
Console.WriteLine($"Stopwatch started");
stopwatch.Start();
for (int i = 0; i <= 10_000_000; i++)
{
    Random random = new Random();
    int randomNumber = random.Next(-10000, 10001);
    numbersList.Add(randomNumber);

    Array.Resize(ref numbersArray, numbersArray.Length + 1);

    numbersArray[i] = randomNumber;
    Console.WriteLine($"new int [{i}] {randomNumber} added");
}
stopwatch.Stop();
Console.WriteLine($"Elapsed Time: {stopwatch.Elapsed.TotalSeconds} seconds");
*/


/*
List<int> numbersList = new List<int>();
Stopwatch stopwatch = new Stopwatch();
Console.WriteLine($"Stopwatch started");
stopwatch.Start();
for (int i = 0; i <= 1_000_000; i++)
{
    Random random = new Random();
    int randomNumber = random.Next(-10000, 10001);
    numbersList.Add(randomNumber);
    Console.WriteLine($"new int [{i}] {randomNumber} added");
}
stopwatch.Stop();
Console.WriteLine($"Elapsed Time: {stopwatch.Elapsed.TotalSeconds} seconds");
*/



//Домашняя работа:
//1.Сделать программу, которая генерирует массив в 10 миллионов случайных чисел от -10000 до +10000.
//Написать программу которая считает сумму этих чисел разделяя на под-массивы в несколько потоков. 
//Программа должна замерять время работы для разного числа потоков. Используйте классы Stopwatch, Task
int[] numbers = new int[1000000];
Random random = new Random();
for (int i = 0; i < numbers.Length; i++)
{
    numbers[i] = random.Next(-10_000, 10_001);
}


Console.WriteLine("Введи количество потоков для первого выполнения:");
var numThreadsString1 = Console.ReadLine();
int numThreadsInt1 = int.Parse(numThreadsString1);
Console.WriteLine("Введи количество потоков для первого выполнения:");
var numThreadsString2 = Console.ReadLine();
int numThreadsInt2 = int.Parse(numThreadsString2);

SumViaTasks(numThreadsInt1);
SumViaTasks(numThreadsInt2);


void SumViaTasks(int threadsCount)
{
    int numThreads = threadsCount;

    Task<long>[] tasks = new Task<long>[numThreads];

    int chunkSize = numbers.Length / numThreads;


    Stopwatch totalStopwatch = new Stopwatch();
    totalStopwatch.Start();

    long totalElapsedTime = 0;
    long totalSum = 0;

    for (int i = 0; i < numThreads; i++)
    {
        int startIndex = i * chunkSize;
        int endIndex = (i == numThreads - 1) ? numbers.Length : (i + 1) * chunkSize;
        int totalTimeUsed = 0;

        tasks[i] = Task.Factory.StartNew(() =>
        {
            Stopwatch taskStopwatch = new Stopwatch();
            taskStopwatch.Start();

            long result = SumChunk(numbers, startIndex, endIndex);
            totalSum += result;

            taskStopwatch.Stop();

            Console.WriteLine($"Task {i + 1} completed in {taskStopwatch.ElapsedMilliseconds} milliseconds");

            totalElapsedTime += taskStopwatch.ElapsedMilliseconds;

            return result;
        });
    }

    // Wait for all tasks to complete
    Task.WaitAll(tasks);

    totalStopwatch.Stop();

    Console.WriteLine($"Sum of task times: {totalElapsedTime} milliseconds");
    // Display the total sum
    Console.WriteLine($"Total Sum: {totalSum}");

}

static long SumChunk(int[] numbers, int startIndex, int endIndex)
{
    long sum = 0;
    for (int i = startIndex; i < endIndex; i++)
    {
        sum += numbers[i];
    }
    return sum;
}
