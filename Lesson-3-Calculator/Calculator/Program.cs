using System;
using System.Globalization;


class Program
{
    static void Main()
    {
        string[] validAnswers = { "+", "-", "*", "/", "%", "√" };
        int maxAttempts = 3;

        do
        {
            int attempts = 0;
            bool usersAnswerValid = false;

            while (!usersAnswerValid && attempts < maxAttempts)
            {
                string options = @"Здравствуйте, выберите операцию из списка:
+
-
*
/
%"
//вопрос про костыль для отображения иконки корня(?)
+ "\n\u221A" + "\n";
                Console.Write(options);
                string usersAnswer = Console.ReadLine();

                if (IsValidInput(usersAnswer, validAnswers))
                {
                    usersAnswerValid = true;

                    switch (usersAnswer)
                    {
                        case "+":
                            {
                                Console.WriteLine("Введите первое значение - либо целое число, либо дробное число с использованием запятой.");
                                decimal var1 = ValidateNumberInput();
                                Console.WriteLine("Введите второе значение - либо целое число, либо дробное число с использованием запятой.");
                                decimal var2 = ValidateNumberInput();
                                decimal result = var1 + var2;
                                Console.WriteLine("Результат:" + result);
                            }
                            break;
                        case "-":
                            {
                                Console.WriteLine("Введите первое значение - либо целое число, либо дробное число с использованием запятой.");
                                decimal var1 = ValidateNumberInput();
                                Console.WriteLine("Введите второе значение - либо целое число, либо дробное число с использованием запятой.");
                                decimal var2 = ValidateNumberInput();
                                decimal result = var1 - var2;
                                Console.WriteLine("Результат:" + result);
                            }
                            break;
                        case "*":
                            {
                                Console.WriteLine("Введите первое значение - либо целое число, либо дробное число с использованием запятой.");
                                decimal var1 = ValidateNumberInput();
                                Console.WriteLine("Введите второе значение - либо целое число, либо дробное число с использованием запятой.");
                                decimal var2 = ValidateNumberInput();
                                decimal result = var1 * var2;
                                Console.WriteLine("Результат:" + result);
                            }
                            break;
                        case "/":
                            {
                                Console.WriteLine("Введите первое значение - либо целое число, либо дробное число с использованием запятой.");
                                decimal var1 = ValidateNumberInput();
                                Console.WriteLine("Введите второе значение - либо целое число, либо дробное число с использованием запятой.");
                                decimal var2 = ValidateSecondNumberForNull();
                                decimal result = var1 / var2;
                                Console.WriteLine("Результат:" + result);
                            }
                            break;
                        case "%":
                            {
                                Console.WriteLine("Введите число, часть от которого вы хотите посчитать");
                                decimal var1 = ValidateSecondNumberForNull();
                                Console.WriteLine("Введите часть от указанного числа, которую вы хотите видеть в процентах");
                                decimal var2 = ValidateNumberInput();
                                decimal result = (var2 / var1) * 100;
                                Console.WriteLine("Результат:" + result + "%");
                            }
                            break;
                        case "√":
                            {
                                Console.WriteLine("Введите число, квадратный корень котоорого вы хотите узнать");
                                decimal var1 = ValidateNumberInput();
                                double result = Math.Sqrt((double)var1);
                                Console.WriteLine("√" + var1 + " = " + result);
                            }
                            break;
                    }
                }
                else
                {
                    attempts++;
                    Console.WriteLine($"Вы ввели неверное значение, попробуйте еще раз. Попыток {attempts}/{maxAttempts}.");
                }
            }

            if (usersAnswerValid)
            {
                Console.Write("Хотите попробовать еще раз? (Да/Нет): ");
                string tryAgainResponse = Console.ReadLine();
                if (tryAgainResponse != "Да")
                {
                    Console.WriteLine("Удачи!");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Хотите продолжить? (Да/Нет): ");
                string continueResponse = Console.ReadLine();
                if (continueResponse != "Да")
                {
                    Console.WriteLine("Удачи!");
                    break;
                }
            }

        } while (true);
    }

    // Function to validate user input against an array of valid values
    static bool IsValidInput(string input, string[] validAnswers)
    {
        foreach (string validValue in validAnswers)
        {
            if (input.Equals(validValue, StringComparison.OrdinalIgnoreCase))
            {
                return true; // Input is valid
            }
        }
        return false; // Input is not valid
    }

    static decimal ValidateNumberInput()
    {
        // Keep asking for input until a valid float is provided
        while (true)
        {
            string input = Console.ReadLine();

            // Check if the input is not empty or whitespace
            if (!string.IsNullOrWhiteSpace(input))
            {
                // Try to parse the input to a float
                if (decimal.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
                {
                    return result; // Exit the loop when a valid float is entered
                }
                else
                {
                    Console.WriteLine("Кажется вы ввели неверные данные, попробуйте ввести число еще раз.");
                }
            }
            else
            {
                Console.WriteLine("Кажется вы ничего не ввели, попробуйте ввести число еще раз.");
            }
        }
    }


    static decimal ValidateSecondNumberForNull()
    {
        // Keep asking for input until a valid float is provided
        while (true)
        {
            string input = Console.ReadLine();

            // Check if the input is not empty or whitespace
            if (!string.IsNullOrWhiteSpace(input))
            {
                // Try to parse the input to a float
                if (decimal.TryParse(input, out decimal result))
                {
                    if (result != 0)
                    {
                        return result; // Exit the loop when a valid float is entered
                    }
                    else
                    {
                        Console.WriteLine("На ноль делить нельзя. Пожауйлста, введите другое число.");
                    }
                }
                else
                {
                    Console.WriteLine("Кажется вы ввели неверные данные, попробуйте ввести число еще раз.");
                }
            }
            else
            {
                Console.WriteLine("Кажется вы ничего не ввели, попробуйте ввести число еще раз.");
            }
        }
    }
}




/*
Console.WriteLine("Введите число, часть от которого вы хотите посчитать");
string var1 = (Console.ReadLine());
float floatvar1 = float.Parse(var1);
decimal decimalVar1 = decimal.Parse(var1);
Console.WriteLine("Введите часть от указанного числа, которую вы хотите видеть в процентах");
string var2 = (Console.ReadLine());
float floatvar2 = float.Parse(var2);
decimal decimalvar2 = decimal.Parse(var2);
float FloatDiv = floatvar2 / floatvar1;
decimal DecimalDiv = decimal.Parse(var2) / decimal.Parse(var1);
float FloatPer = FloatDiv * 100;
decimal DecPer = DecimalDiv * 100;
Console.WriteLine("Результат деления float:" + FloatDiv);
Console.WriteLine("Результат деления decimal:" + DecimalDiv);
Console.WriteLine("Результат % float:" + FloatPer);
Console.WriteLine("Результат % decimal:" + DecPer);
*/