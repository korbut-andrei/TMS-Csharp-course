// See https://aka.ms/new-console-template for more information

using System;

string[] validAnswers = { "+", "-", "*", "/", "%", "√" };
int errorsMadeCount = 0;
bool answerIsValid = false;
while (answerIsValid == false && errorsMadeCount < 3)
{
    string Answer = Console.ReadLine();
    int matchCount = 0;
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
    Console.WriteLine("cope");
}
else
{
    Console.WriteLine("fail");
}

/*class Program
{
    static void Main()
    {
        Console.WriteLine("Добрый день, меня зовут Петр, я чат-бот TeachMeSkills, который поможет сформировать заявку на Ваше обучение.");
        Console.WriteLine("Как Вас зовут?");
        var customersName = Console.ReadLine();
        Console.WriteLine("Приятно познакомиться, " + customersName);
        Console.WriteLine("Пожалуйста, выберите курс на который вы хотели бы зарегистрироваться из данного списка:");
        Console.WriteLine("C#");
        Console.WriteLine("Javascript");
        Console.WriteLine("Python");
        Console.WriteLine("Ruby");
        Console.WriteLine("Go");
        Console.WriteLine("Введите ответ ниже:");
        string[] validAnswers = { "C#", "Javascript", "Python", "Ruby", "Go" };
        string customersAnswer = Console.ReadLine();
        int errorsMadeCount = 0;
        bool answerIsValid = false;
        int matchCount = 0;
        // Get the current date and time
        DateTime currentDate = DateTime.Now;
        // Get the day of the week as an enumeration value
        DayOfWeek currentDayOfWeek = currentDate.DayOfWeek;
        // Convert the enumeration value to the day name
        string currentDayName = currentDayOfWeek.ToString();
        while (answerIsValid == false && errorsMadeCount < 3)
        {
            foreach (string validValue in validAnswers)
            {
                if (customersAnswer == validValue)
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
                Console.WriteLine("Введенный ответ не совпадает с названиями курсов. Пожалуйста, попробуйте еще раз.");
                customersAnswer = Console.ReadLine();
            }
        }
        foreach (string validValue in validAnswers)
        {
            if (customersAnswer == validValue)
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
            answerIsValid = false;
        }
        if (answerIsValid == true)
            {
                Console.WriteLine("Поздравляю, " + customersName + ". Вы записаны на " + customersAnswer + ". Наш менеджер свяжется с Вами, чтобы уточнить детали.");
            }
            else
            {
             if (currentDayName != "Sunday" && currentDayName !=  "Saturday" )
                {
                Console.WriteLine("У нас не получается определить курс, который вы выбрали. Пожалуйста, обратитесь в тех поддержку по телефону +375 29 333 33 33");
                 }
             else
                {
                Console.WriteLine("Сегодня в нашей компании выходной. Пожалуйста, обратитесь в тех поддержку в понедельник.");
                 }
            }
        
    }
}
*/