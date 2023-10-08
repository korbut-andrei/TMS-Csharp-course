
class Program
{
    static void Main()
    {
        string options = @"Здравствуйте, выберите операцию из списка:
+
-
*
/
%"
//вопрос про костыль для отображения иконки корня(?)
+ "\n\u221A";
        Console.WriteLine(options);
        //Console.OutputEncoding = System.Text.Encoding.UTF8;
        //Console.WriteLine("\u221A");
        //\u221A16
        //Отображение результата

        string[] validAnswers = { "+", "-", "*", "/", "%", "√" };
        int errorsMadeCount = 0;
        bool answerIsValid = false;
        int matchCount = 0;
        while (answerIsValid == false && errorsMadeCount < 3)
        {
            string Answer = Console.ReadLine();
            //string test = Console.ReadLine();
            foreach (string validValue in validAnswers)
            {
                if (Answer == validValue)
                {
                    matchCount++;
                }
            }
            // Check if there is exactly one match
            if (matchCount == 1)
            {
                answerIsValid = true;
            }
            else
            {
                errorsMadeCount++;
                Console.WriteLine("Введенный ответ не совпадает с предложенными опциями, пожалуйста, проверьте правильность написания мат. операции и попробуйте еще раз.");
            }
        }
        if (answerIsValid == true)
        {
            switch (Answer)
            {
                case "+":
                    {
                        Console.WriteLine("Введи первое число");
                        string? s1 = Console.ReadLine();
                        Console.WriteLine("Введи второе число");
                        string? s2 = Console.ReadLine();
                        var result = s1 + s1;
                        Console.WriteLine("Сумма:" + result);
                        if (s1 == null)
                        {
                            Console.WriteLine("null");
                        }

                        if (s1 == "")
                        {
                            Console.WriteLine("empty string");
                        }
                        break;
                    }
                default:
                    Console.WriteLine("Операция не поддерживается");
                    break;
            }
        }
        else
        {
           
        }

    }
}






/*
{
    string options = @"Здравствуйте, выберите операцию из списка:
+
-
*
/
%" 
+"\n\u221A";
    Console.WriteLine(options);
    //Console.OutputEncoding = System.Text.Encoding.UTF8;
    //Console.WriteLine("\u221A");
//\u221A16
//Отображение результата

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "+":
            {
                Console.WriteLine("Введи первое число");
                string? s1 = Console.ReadLine();
                Console.WriteLine("Введи второе число");
                string? s2 = Console.ReadLine();
                var result = s1 + s1;
                Console.WriteLine("Сумма:" + result);
                if (s1 == null)
                {
                    Console.WriteLine("null");
                }

                if (s1 == "")
                {
                    Console.WriteLine("empty string");
                }
                break;
            }
        case "-":
            {
                Console.WriteLine("Введи первое число");
                string? s1 = Console.ReadLine();
                Console.WriteLine("Введи второе число");
                string? s2 = Console.ReadLine();
                var result = s1 + s1;
                Console.WriteLine("Сумма:" + result);
                if (s1 == null)
                {
                    Console.WriteLine("null");
                }

                if (s1 == "")
                {
                    Console.WriteLine("empty string");
                }
                break;
            }
        case "*":
            {
                Console.WriteLine("Введи первое число");
                string? s1 = Console.ReadLine();
                Console.WriteLine("Введи второе число");
                string? s2 = Console.ReadLine();
                var result = s1 + s1;
                Console.WriteLine("Сумма:" + result);
                if (s1 == null)
                {
                    Console.WriteLine("null");
                }

                if (s1 == "")
                {
                    Console.WriteLine("empty string");
                }
                break;
            }
        default:
            Console.WriteLine("Операция не поддерживается");
                break;
    }
}

static uint ReadUint()
{ 
while (true) 
    {
        var multAsText = Console.ReadLine();
        uint number;
        bool isParsed = uint.TryParse(multAsText, out number);
        if (isParsed)
        {
            return number;
        }
        Console.WriteLine("Введи целое число"); 
    }
}

static string ReadNotEmptyString()
{
    while (true)
    {
        var input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            return input;
        }
        Console.WriteLine("Введите целое или дроное число с использованием запятой");
    }
}

static bool DoOperation()
{
    Console.WriteLine("Введи операцию");
    var operation = Console.ReadLine();
    switch (operation) 
    { 
    
    }
    return true;
}

static bool DoOperations()
{
    while (true)
    {
        var result = DoOperation();
        if (!result) 
        { 
        Console.WriteLine("Ты ввел не то");
            break;
        }

    }
    Console.WriteLine("Poka");
    Console.ReadLine();
}
*/