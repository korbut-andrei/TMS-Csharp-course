// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using System.Xml.Linq;


//Основное задание
//1. Класс Phone.
//Создайте класс Phone, который содержит переменные number, model и weight.
//Создайте три экземпляра этого класса.
//Выведите на консоль значения их переменных.

Phone iPhone = new Phone("44-765-33-90", "iPhone 13", 221);
Phone Samsung = new Phone("44-333-33-44", "Samsung Galaxy s24", 240);
Phone Xiaomi = new Phone("44-231-11-31", "Xiaomi 12 PRO", 211);
Phone[] phones = { iPhone, Samsung, Xiaomi };

foreach (Phone phone in phones)
{
    Console.WriteLine($"Number: {phone.Number}");
    Console.WriteLine($"Model: {phone.Model}");
    Console.WriteLine($"Weight: {phone.Weight}\n");
}
foreach (Phone phone in phones)
{
   phone.receiveCall();

   phone.getNumber();
}


//Вызвать это метод.
Random random = new Random();
int randomIndexPhones = random.Next(phones.Length);
phones[randomIndexPhones].receiveCallLoaded("Джон", "44-231-11-31");
phones[randomIndexPhones].sendMessage(new string[] { "44-333-22-31", "44-111-33-22", "44-441-31-99" });

//Добавить в класс Phone методы: receiveCall, имеет один параметр – имя звонящего.
//Выводит на консоль сообщение “Звонит {name}”. 
//getNumber – возвращает номер телефона. 
//Вызвать эти методы для каждого из объектов.
class Phone
{
    public string Number { get; set; }
    public string Model { get; set; }
    public decimal Weight { get; set; }
    private string[] possibleNames = { "Николай", "Сергей", "Алексей", "Афанасий", "Василий" };
    private string[] possibleMessages = { "Hello", "How are you?", "Meet me at 5 PM", "Call me back", "I'll be there in 10 minutes" };

    //Добавить конструктор в класс Phone, который принимает на вход три параметра
    //для инициализации переменных класса - number, model и weight.
    //Вызвать из конструктора с тремя параметрами конструктор с двумя.
    public Phone(string number, string model, decimal weight) : this(number, model)
    {
        Weight = weight;
    }

    //Добавить конструктор, который принимает на вход два параметра
    //для инициализации переменных класса - number, model.
    public Phone(string number, string model)
    {
        Number = number;
        Model = model;
    }

    //Добавить конструктор без параметров.
    public Phone()
    {
        // Default constructor with no parameters
    }

    public void receiveCall()
    {
        Random random = new Random();
        int randomIndex = random.Next(possibleNames.Length);
        string callerName = possibleNames[randomIndex];
        Console.WriteLine($"Звонит {callerName}");
    }

    public void getNumber()
    {
        Console.WriteLine($"Номер телефона {Number}\n");
    }

    //Добавьте перегруженный метод receiveCall, который принимает два параметра - имя звонящего и номер телефона звонящего.
    public void receiveCallLoaded(string callername, string phoneNumber)
    {
        Console.WriteLine($"Звонит {callername}, номер {phoneNumber}");
    }

    //Создать метод sendMessage с аргументами переменной длины.
    //Данный метод принимает на вход номера телефонов, которым будет отправлено сообщение.
    //Метод выводит на консоль номера этих телефонов.
    public void sendMessage(string[] phoneNumbers)
    {
        Random random = new Random();

        foreach (string phoneNumber in phoneNumbers)
        {
            int randomIndex = random.Next(possibleMessages.Length);
            string message = possibleMessages[randomIndex];
            Console.WriteLine($"Отправка сообщения '{message}' на {phoneNumber}");
        }
    }
}

//2.Создать программу для имитации работы клиники.
//Пусть в клинике будет три врача: хирург, терапевт и дантист.




Patient Patient1 = new Patient("Дмитрий", "Денисов");
Patient1.DisplayRegistrationMessage();
Physician Physician1 = new Physician("Виталий", "Краснов");
Dentist Dentist1 = new Dentist("Александр", "Кучеров");
Surgeon Surgeon1 = new Surgeon("Алексей", "Дерепаев");
therapyPlan therapyPlan1 = new therapyPlan("План лечения простуды", 3);
therapyPlan therapyPlan2 = new therapyPlan("Лечение ишемической болезни", 1);
therapyPlan therapyPlan3 = new therapyPlan("Ортодонтическое лечение", 2);
//Создать объект класса «Пациент» и добавить пациенту план лечения.
Physician1.AssignTherapyPlan(Patient1, therapyPlan1);
assignDoctorToPatient(therapyPlan3, Patient1, Physician1, Dentist1, Surgeon1);



static void assignDoctorToPatient(therapyPlan therapyPlanSent, Patient patientReceived, Physician physician, Dentist dentist, Surgeon surgeon)
{
    //Так же создать метод, который будет назначать врача пациенту согласно плану лечения.
    //Если план лечения имеет код 1 – назначить хирурга и выполнить метод лечить.
    //Если план лечения имеет код 2 – назначить дантиста и выполнить метод лечить.
    //Если план лечения имеет любой другой код – назначить терапевта и выполнить метод лечить.
    if (therapyPlanSent.Code == 1)
    {
        patientReceived.AssignDoctor(surgeon);
        surgeon.treatPatient(patientReceived);
    }
    else if (therapyPlanSent.Code == 2)
    {
        patientReceived.AssignDoctor(dentist);
        dentist.treatPatient(patientReceived);
    }
    else
    {
        patientReceived.AssignDoctor(physician);
        physician.treatPatient(patientReceived);
    }
}


public abstract class Doctor
{

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Specialization { get; set; }

    public Doctor(string name, string surname, string specialization)
    {
        Specialization = specialization;
        Name = name;
        Surname = surname;
    }
    //Каждый врач имеет метод «лечить», но каждый врач лечит по-своему.
    public abstract void treatPatient(Patient patientName);

    public void AssignTherapyPlan(Patient patient, therapyPlan therapyPlanSent)
    {
        patient.AssignTherapyPlan(therapyPlanSent);
    }

}

//хирург
public class Surgeon : Doctor
{
    public Surgeon(string name, string surname) : base(name, surname, "Хирург") { }

    public override void treatPatient(Patient patientName)
    {
        Console.WriteLine($"{Name} {Surname} оперирует {patientName.Name}{patientName.Surname}.\n");
    }
}

//терапевт
public class Physician : Doctor
{
    public Physician(string name, string surname) : base(name, surname, "Терапевт") { }

    public override void treatPatient(Patient patientName)
    {
        Console.WriteLine($"{Name} {Surname} назначает лечение {patientName.Name + Surname}.\n");
    }
}

//дантист
public class Dentist : Doctor
{
    public Dentist(string name, string surname) : base(name, surname, "Стоматолог") { }

    public override void treatPatient(Patient patientName)
    {
        Console.WriteLine($"{Name} {Surname} лечит зубы {patientName.Name} {patientName.Surname}.\n");
    }
}

//Так же предусмотреть класс «Пациент» 
public class Patient
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public therapyPlan therapyPlan { get; private set; }
    public Doctor assignedDoctor { get; private set; }
    public Patient(string name, string surname)
    {
        Name = name;
        Surname = surname;
    }

    public void DisplayRegistrationMessage()
    {
        Console.WriteLine($"Пациент {Name} {Surname} зарегистрирован.");
    }
    public void AssignTherapyPlan(therapyPlan therapyPlanSent)
    {
        therapyPlan = therapyPlanSent;
        Console.WriteLine($"Пациенту {Name} {Surname} назначен план терапии - {therapyPlan.Description}\n");
    }

    public void AssignDoctor(Doctor doctorReceived)
    {
        assignedDoctor = doctorReceived;
        Console.WriteLine($"Пациенту {Name} {Surname} назначен доктор {doctorReceived.Name}{doctorReceived.Surname}\n");
    }
}
//и класс «План лечения».
public class therapyPlan
{
    public int Code { get; set; }
    public string Description { get; set; }
    public therapyPlan(string description, int code)
    {
        Description = description;
        Code = code;
    }
}


