//Основное задание
//1. Создать класс CreditCard c полями номер счета, текущая сумма на счету.



using System;
using System.Xml.Linq;


//Тестовый сценарий для проверки:
//Напишите программу, которая создает три объекта класса CreditCard у которых заданы номер счета и начальная сумма
//Положите деньги на первые две карточки и снимите с третьей.
//Выведите на экран текущее состояние всех трех карточек.

List<Account> accounts = new List<Account>();
for (int i = 0; i < 3; i++)
{
    Account newAccount = new Account();
    accounts.Add(newAccount);
}

accounts[0].Deposit(13);

accounts[1].Deposit(121);


accounts[2].Withdraw(accounts[2].AccountBalance-1);

for (int i = 0; i < 3; i++)
{
    accounts[i].DisplayBalance();
}

public class Account
{
    public int AccountNumber {  get; set; }
    public decimal AccountBalance { get; set; }

    public Account()
    {
        decimal minValue = 1.0m; // Minimum value of the range
        decimal maxValue = 10.0m; // Maximum value of the range
        decimal randomDecimal = Math.Abs(GenerateRandomDecimalInRange(minValue, maxValue));
        
        static decimal GenerateRandomDecimalInRange(decimal minValue, decimal maxValue)
        {
            Random random = new Random();
            double randomDouble = random.NextDouble();
            decimal range = maxValue - minValue;

            return minValue + (decimal)(randomDouble * (double)range);
        }

        Random random = new Random();
        int minValueInt = 1;
        int maxValueInt = 1000; 
        int randomNInt = random.Next(minValueInt, maxValueInt + 1);

        AccountNumber = randomNInt;
        AccountBalance = randomDecimal;
    
    }
    //Добавьте метод, который позволяет начислять сумму на кредитную карточку.
    public void Deposit (decimal amount)
    {
        if (amount > 0 )
        {
            AccountBalance += amount;
            Console.WriteLine($"Deposited {amount:C}. New balance: {AccountBalance:C}");
        }
        else
        {
            Console.WriteLine("Invalid deposit amount. Amount must be greater than 0.");
        }
    }
    //Добавьте метод, который позволяет снимать с карточки некоторую сумму.
    public void Withdraw(decimal amount )
    {
        if (amount > 0 && AccountBalance >= amount)
        {
            AccountBalance -= amount;
            Console.WriteLine($"Withdrew {amount:C}. New balance: {AccountBalance:C}");
        }
        else if (AccountBalance >= amount)
        {
            Console.WriteLine("There's not enough funds on your account");
        }
        else
        {
            Console.WriteLine("You can withdraw only amount above 0");
        }
    }
    //Добавьте метод, который выводит текущую информацию о карточке.
    public void DisplayBalance()
    {
        Console.WriteLine($"Account {AccountNumber}. Current balance  = {AccountBalance:C}");
    }
}




//Тестовый сценарий для проверки:
//создать объект "компьютер 1" с помощью первого конструктора и
//вывести информацию на экран;
//создать объект "компьютер 2" с помощью второго конструктора и
//вывести информацию на экран.

PC pc1 = new PC(321.3m, "ACER Predator Orion 7000 PO7-640");
pc1.provideInformation();
PC pc2 = new PC(278.4m, "ACTINA GeForce RTX Studio", new RAM("8GB", 8192), new HDD("Dell Samsung 3.2TB PCIe 2.5 NVMe", 3000, HDDType.Internal));
pc2.provideInformation();

//Дополнительное задание
//Создать класс для описания компьютера, в этом классе должны быть
//поля: стоимость, модель(строковый тип), RAM и HDD.
public class PC
{
    public decimal Price { get; set; }
    public string Model { get; set; }
    public RAM RAM { get; set; }
    public HDD HDD { get; set; }
    //Класс Computer должен иметь два конструктора:
    //-первый - с параметрами стоимость и модель,
    //- второй - со всеми полями.
    public PC(decimal price, string model, RAM ram, HDD hdd) : this(price, model)
    {
        RAM = ram;
        HDD = hdd;
    }
    public PC(decimal price, string model)
    {
        Price = price;
        Model = model;
        RAM = new RAM();
        HDD = new HDD();
    }

    public void provideInformation()
    {
        Console.WriteLine($"PC: {Model} \nPrice: {Price} \nRAM: {RAM.Name} / {RAM.Capacity} \nHDD: {HDD.Name} / {HDD.Capacity}\n");
    }
}

//Для полей RAM и HDD следует создать свои собственные классы.
//Классы для RAM и HDD должны иметь конструктор по умолчанию и конструктор со всеми параметрами.
public class RAM
{
    //Класс RAM имеет поля "название" и "объем".
    public string Name { get; set; }
    public int Capacity { get; set; }

    public RAM(string name, int capacity)
    {
        Name = name;
        Capacity = capacity;
    }

    public RAM()
    {
        Name = "Undefined";
        Capacity = 0;
    }

    public void provideInformation()
    {
        Console.WriteLine($"RAM: {Name} \nCapacity: {Capacity}");
    }
}

public enum HDDType
{
    Internal,
    External,
    Undefined
}

public class HDD
{
    //Класс HDD имеет поля "название", "объем" и "тип" (внешний или внутренний).
    public string Name { get; set; }
    public int Capacity { get; set; }
    public HDDType Type { get; set; }
    public HDD(string name, int capacity, HDDType type)
    {
        Name = name;
        Capacity = capacity;
        Type = type;
    }

    public HDD()
    {
        Name = "Undefined";
        Capacity = 0;
        Type = HDDType.Undefined;
    }

    public void provideInformation()
    {
        Console.WriteLine($"HDD: {Name} \nCapacity: {Capacity} \nType: {Type}");
    }
}

//2. Создать класс, описывающий банкомат.
//Набор купюр, находящихся в банкомате, должен задаваться тремя
//свойствами:
//количеством купюр номиналом 20, 50 и 100.
//Сделать метод для добавления денег в банкомат.
//Сделать функцию, снимающую деньги, которая принимает сумму денег, а
//возвращает булевое значение - успешность выполнения операции.
//При снятии денег функция должна распечатывать каким количеством
//купюр какого номинала выдаётся сумма.
//Создать конструктор с тремя параметрами - количеством купюр каждого номинала

class 