
//task 1
using System;
using System.Drawing;
using System.Security.Cryptography;


int number = 1;
for (int i = 1; i < 51; i++)
{
    Console.WriteLine(number);
    number += 2;
}

Console.ReadLine();

//task 2
for (int a = 5; a > 0; a--)
{
    Console.WriteLine(a);
}

Console.ReadLine();

//task 3
var userNumber = Console.ReadLine();
int userNumberInt = 1;
try
{
    userNumberInt = int.Parse(userNumber);
    Console.WriteLine($"Полученное число: " + userNumberInt);
}
catch (FormatException)
{
    Console.WriteLine("Неверный ввод");
}

int sum = 0;
string numbersString = ""; 
for (int b = 1; b <= userNumberInt; b++)
{
    sum += b;
    numbersString += b;
    numbersString += " + ";
}
Console.WriteLine(numbersString + " = " + sum);
Console.ReadLine();

//task 4
int i4 = 7;
string numbersString4 = "";
while (i4 <= 98)
{
    numbersString4 += i4;
    i4 += 7;
    numbersString4 += "  ";
}
Console.WriteLine(numbersString4);
Console.ReadLine();

//task 5
//5.Вывести 10 первых чисел последовательности 0, -5,-10,-15..
int i5 = 0;
int numberCount5 = 1;
string numbersString5 = "";
while (numberCount5 <= 10)
{
    i5 -= 5;
    numberCount5++;
    numbersString5 += i5 + "  ";
}
Console.WriteLine(numbersString5);
Console.ReadLine();



//task 6
//Составьте программу, выводящую на экран квадраты чисел от 10 до 20 включительно.
int numberCount6 = 1;
string numbersString6 = "";
for (int i6 = 10; i6 <= 20; i6++)
{
    numbersString6 += i6 + "*2 = " + (i6 * i6);
    Console.WriteLine(numbersString6);
    numbersString6 = "";
}
Console.ReadLine();

//0.Создайте массив целых чисел. Напишете программу, которая выводит сообщение о том, входит ли заданное число в массив или нет.
//Пусть число для поиска задается с консоли (класс Scanner).
int[] nums7 = { 1, 2, 3, 5, 45, 96 };
string userInput7 = Console.ReadLine();
bool isInputValid7 = Scanner(nums7, userInput7);
if (isInputValid7)
{
    Console.WriteLine("Введенное вами число входит в массив");
}
else
{
    Console.WriteLine("Введенное вами число НЕ входит в массив");
}


static bool Scanner(int[] array, string userInput)
{
    if (int.TryParse(userInput, out int inputNumber))
    {
        foreach (int number in array)
        {
            if (inputNumber == number)
            {
                return true;
            }
        }
    }
    return false;
}
Console.ReadLine();

//1.Создайте массив целых чисел. Удалите все вхождения заданного числа из массива.
//Пусть число задается с консоли (класс Scanner). Если такого числа нет - выведите сообщения об этом.
//В результате должен быть новый массив без указанного числа.
int[] numbers = { 1, 2, 3, 4, 5 };
while (true)
{
    Console.Write("Введите число для проверки: ");
    if (int.TryParse(Console.ReadLine(), out int numberToRemove))
    {
        if (numbers.Contains(numberToRemove))
        {
            int[] newArray = numbers.Where(n => n != numberToRemove).ToArray();

            Console.WriteLine("Изначальный массив: " + string.Join(", ", numbers));
            Console.WriteLine("Новый массив после удаления " + numberToRemove + ": " + string.Join(", ", newArray));
            break;
        }
        else
        {
            Console.WriteLine("Число не входит в массив");
        }
    }
    else
    {
        Console.WriteLine("Введите целое число");
    }
}
Console.ReadLine();

//2. Создайте и заполните массив случайным числами и выведете максимальное, минимальное и среднее значение.
//Для генерации случайного числа используйте метод Math.random().
//Пусть будет возможность создавать массив произвольного размера.
//Пусть размер массива вводится с консоли.

        while (true)
        {
            Console.Write("Введите размер массива: ");
            if (int.TryParse(Console.ReadLine(), out int arraySize))
            {
                int[] newArray8 = new int[arraySize];
                for (int b8 = 0; b8 < arraySize; b8++)
                {
                    // Generate a new random number for each element
                    newArray8[b8] = GenerateRandomInteger();
                }
                string result8 = string.Join(", ", newArray8);
                Console.WriteLine(result8);
                Console.WriteLine("Минимальное значение массива: " + FindMin(newArray8));
                Console.WriteLine("Максимальное значение массива: " + FindMax(newArray8));
                Console.WriteLine("Среднее значение массива: " + FindAverage(newArray8));
                break;
            }
            else
            {
                Console.WriteLine("Введите целое число");
            }
        }
        Console.ReadLine();
    

    static int FindMin(int[] array)
    {
        int min = array[0]; // Assume the first element is the minimum
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] < min)
            {
                min = array[i];
            }
        }
        return min;
    }

    static int FindMax(int[] array)
    {
        int max = array[0]; // Assume the first element is the maximum
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > max)
            {
                max = array[i];
            }
        }
        return max;
    }

    static double FindAverage(int[] array)
    {
        int sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return (double)sum / array.Length;
    }

    static int GenerateRandomInteger()
    {
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] randomBytes = new byte[4]; // 4 bytes for a 32-bit integer
            rng.GetBytes(randomBytes);
            int randomNumber = BitConverter.ToInt32(randomBytes, 0);
            return randomNumber;
        }
    }

//3. Создайте 2 массива из 5 чисел.
//Выведите массивы на консоль в двух отдельных строках.
//Посчитайте среднее арифметическое элементов каждого массива и сообщите,
//для какого из массивов это значение оказалось больше (либо сообщите, что их средние арифметические равны).
int[] newArray9 = { 1, 2, 3, 4, 5 };
int[] newArray10 = { 1, 2, 3, 4, 5 };
string result9 = string.Join(", ", newArray9);
string result10 = string.Join(", ", newArray10);
Console.WriteLine(result9);
Console.WriteLine(result10);
double avg1 = newArray9.Average();
double avg2 = newArray10.Average();
double highestAverage = Math.Max(avg1, avg2);
Console.WriteLine("Среднее значение массива 1 = " + avg1);
Console.WriteLine("Среднее значение массива 2 = " + avg2);
if (avg1 != avg2)
{
    Console.WriteLine("Наибольшее среднее значения = " + Math.Max(avg1, avg2));
}
else
{
    Console.WriteLine("Средние значения равны");
}