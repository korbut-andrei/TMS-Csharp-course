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
string sumText = ;
for (int b = 1; b < userNumberInt; b++)
{
    sum += b  ;
}
Console.WriteLine("Сумма числе до числа " + userNumberInt + " = " + sum);

