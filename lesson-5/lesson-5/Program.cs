using System;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.RegularExpressions;

do 
{ 
    Console.WriteLine("Введите строку:");
    string userInputString = Console.ReadLine() ?? "";
    var userInputStringToArray = userInputString.Split(" ");
    Console.WriteLine("Введите порядковый номер действия, которое вы хотите выполнить с введенной строкой: " +
        "\n1. - Найти слова, содержащие максимальное количество цифр." +
        "\n2. - Найти самое длинное слово и определить, сколько раз оно встретилось в тексте." +
        "\n3. - Вывести на экран сначала вопросительные, а затем восклицательные предложения." +
        "\n4. - Вывести на экран только предложения, не содержащие запятых." +
        "\n5. - Найти слова, начинающиеся и заканчивающиеся на одну и ту же букву.");
    string usersChoice = Console.ReadLine();
    switch (usersChoice)
    {
        case "1":
            {
                var userInputStringToArrayWithoutNulls = FilterNullAndWhitespace(userInputStringToArray);
                var MaxDigitsInUserInput = MaxDigitsCount(userInputStringToArrayWithoutNulls);
                var ArrayIncludingOnlyMaxDigits = StringsWithMostDigits(userInputStringToArrayWithoutNulls, MaxDigitsInUserInput);
                Console.WriteLine($"Эти слова содержат наибольшее количество цифр: {string.Join(", ", ArrayIncludingOnlyMaxDigits)}");
            }
            break;
        case "2":
            {
                var userInputStringToArrayWithoutNulls = FilterNullAndWhitespace(userInputStringToArray);
                var userInputArrayWithoutPunctuation = RemovePunctuationFromArray(userInputStringToArrayWithoutNulls);
                int MaxCharCount = 0;
                foreach (string str in userInputStringToArrayWithoutNulls)
                {
                    int charCount = 0;
                    foreach (char c in str)
                    {   if (!char.IsPunctuation(c))
                        {
                            charCount++;
                        }
                    }
                    if (charCount > MaxCharCount)
                    {
                        MaxCharCount = charCount;
                    }
                }
                var ArrayIncludingOnlyMaxChars = userInputArrayWithoutPunctuation.Where(str => str.Length == MaxCharCount).ToArray();
                Dictionary<string, int> WordsRepetitions = new Dictionary<string, int>();
                foreach (string MaxCharStr in ArrayIncludingOnlyMaxChars)
                {
                    int count = userInputArrayWithoutPunctuation.Count(RegularStr => RegularStr == MaxCharStr);
                    WordsRepetitions[MaxCharStr] = count;
                }
                foreach (var word in WordsRepetitions)
                {
                    Console.WriteLine($"'{word.Key}' появляется в тексте {word.Value} раз.");
                }

            }
            break;
            //3. - Вывести на экран сначала вопросительные, а затем восклицательные предложения.
        case "3":
            {
                string[] sentences = SeparateIntoSentences(userInputString);
                var questions = new System.Collections.Generic.List<string>();
                var exclamations = new System.Collections.Generic.List<string>();
                foreach (string sentence in sentences)
                {
                    // Check the last character of the sentence
                    char lastChar = sentence.Trim().LastOrDefault();
                    if (lastChar == '?')
                        questions.Add(sentence);
                    else if (lastChar == '!')
                        exclamations.Add(sentence);
                }
                
                // Print questions first, followed by exclamations
                Console.WriteLine("Вопросительные предложения:");
                foreach (string question in questions)
                {
                    Console.WriteLine(question);
                }
                
                Console.WriteLine("\nВосклицательные предложения:");
                foreach (string exclamation in exclamations)
                {
                    Console.WriteLine(exclamation);
                }
                break;
            }
        //Вывести на экран только предложения, не содержащие запятых
        case "4":
            {
                string[] sentences = SeparateIntoSentences(userInputString);
                var sentencesWithoutCommas = new System.Collections.Generic.List<string>();
                foreach (string str in sentences)
                {
                    bool commaIsPresent = false;
                    foreach (char c in str)
                        if (c == ',')
                        {
                            commaIsPresent = true;
                            break;
                        }
                    if (!commaIsPresent)
                    {
                        sentencesWithoutCommas.Add(str);
                    }
                }
                if (sentencesWithoutCommas.Count != 0)
                {
                    Console.WriteLine("Предложения без запятых:");
                    foreach (string sentenceWithoutCommas in sentencesWithoutCommas)
                    {
                        Console.WriteLine(sentenceWithoutCommas);
                    }
                }
                else
                {
                    Console.WriteLine("В тексте нету предложений без запятых.");
                }
                break;
            }
        //Найти слова, начинающиеся и заканчивающиеся на одну и ту же букву
        case "5":
            {
                var userInputStringToArrayWithoutNulls = FilterNullAndWhitespace(userInputStringToArray);
                var userInputArrayWithoutPunctuation = RemovePunctuationFromArray(userInputStringToArrayWithoutNulls);
                var wordsWithSameLetter = new System.Collections.Generic.List<string>();
                foreach (string str in userInputArrayWithoutPunctuation)
                {
                    int strLength = str.Length;
                    if (strLength > 0 && str[0] == str[strLength - 1])
                    {
                        wordsWithSameLetter.Add(str);
                    }
                }
                if (wordsWithSameLetter.Count != 0)
                {
                    Console.WriteLine("Слова с одинаковой буквой:");
                    foreach (string sentence in wordsWithSameLetter)
                    {
                        Console.WriteLine(sentence);
                    }
                }
                else
                {
                    Console.WriteLine("В тексте нету слов с одинаковой буквой.");
                }
                break;
            }
        default:
            Console.WriteLine("Вы ошиблись со вводом, попробуйте еще раз.");
            break;
    }
}while (true);



static string[] SeparateIntoSentences(string str)
{
    string sentencePattern = @"(?<=[.!?])\s+";
    string[] sentences = Regex.Split(str, sentencePattern);
    return sentences;
}

    static string RemovePunctuation(string input)
{
    // Create a StringBuilder to build the cleaned string
    StringBuilder cleanedString = new StringBuilder();

    foreach (char c in input)
    {
        if (!char.IsPunctuation(c))
        {
            // Append non-punctuation characters to the cleaned string
            cleanedString.Append(c);
        }
    }

    return cleanedString.ToString();
}

static string[] RemovePunctuationFromArray(string[] array)
{
    string[] cleanedArray = new string[array.Length];

    for (int i = 0; i < array.Length; i++)
    {
        cleanedArray[i] = RemovePunctuation(array[i]);
    }

    return cleanedArray;
}

static string[] StringsWithMostDigits(string[] inputArray, int MaxDigits)
{
    int count = 0;
    foreach (string c in inputArray)
    {
        if (DigitCount(c) == MaxDigits)
        {
            count++;
        }
    }

    string[] OnlyMaxDigitsArray = new string[count];
    int index = 0;
    for (int i = 0; i < inputArray.Length; i++)
    {
        if (DigitCount(inputArray[i]) == MaxDigits)
        {
            OnlyMaxDigitsArray[index] = inputArray[i];
            index++;
        }
    }
    return OnlyMaxDigitsArray;
}

static int MaxDigitsCount(string[] inputArray)
{
    int MaxCount = 0;
    foreach (string c in inputArray)
    {
        int count = DigitCount(c);
        if (count > MaxCount)
        {
            MaxCount = count;
        }
    }
    return MaxCount;
}


static string[] FilterNullAndWhitespace(string[] inputArray)
{
    int count = 0;

    // Count the non-null and non-whitespace strings
    for (int i = 0; i < inputArray.Length; i++)
    {
        if (!MyIsNullOrWhiteSpace(inputArray[i]))
        {
            count++;
        }
    }

    // Create a new array to hold the filtered strings
    string[] filteredArray = new string[count];

    // Copy the non-null and non-whitespace strings to the filtered array
    int index = 0;
    for (int i = 0; i < inputArray.Length; i++)
    {
        if (!MyIsNullOrWhiteSpace(inputArray[i]))
        {
            filteredArray[index] = inputArray[i];
            index++;
        }
    }
    return filteredArray;
}


static bool MyIsNullOrWhiteSpace(string input)
{
    if (input == null)
        return true;

    foreach (char c in input)
    {
        if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
            return false;
    }
    return true;
}

static int DigitCount(string input)
{
    int DigitsCount = 0;
    char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    foreach (char c in input)
        foreach (char digit in digits)
        {
            if (c == digit)
            {
                DigitsCount++;
            }

        }
    return DigitsCount;
}


//-Заменить цифры от 0 до 9 на слова «ноль», «один», ..., «девять».
//- Вывести на экран сначала вопросительные, а затем восклицательные предложения.
//- Вывести на экран только предложения, не содержащие запятых.
//- Найти слова, начинающиеся и заканчивающиеся на одну и ту же букву.
//Приложение не должно падать ни при каких условиях.