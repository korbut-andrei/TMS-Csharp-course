using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_7
{

    ATM atm1 = new ATM(5, 5, 5);
    var str1 = Console.ReadLine();
    if (int.TryParse(str1, out int number))
    {
    }
    else
    {
    Console.WriteLine("Invalid input. Please enter a valid integer.");
    }
    atm1.WithdrawCash(number);


//2. Создать класс, описывающий банкомат.
public class ATM
{
    //Набор купюр, находящихся в банкомате, должен задаваться тремя
    //свойствами:
    //количеством купюр номиналом 20, 50 и 100.
    public int TwentyDollarBills { get; set; }
    public int FiftyDollarBills { get; set; }
    public int OneHundredDollarBills { get; set; }

    //Создать конструктор с тремя параметрами - количеством купюр каждого номинала

    public ATM(int twentyDollarBills, int fiftyDollarBills, int oneHundredDollarBills)
    {
        TwentyDollarBills = twentyDollarBills;
        FiftyDollarBills = fiftyDollarBills;
        OneHundredDollarBills = oneHundredDollarBills;
    }

    //Сделать метод для добавления денег в банкомат.
    public void AddCash(int twentyDollarBills, int fiftyDollarBills, int oneHundredDollarBills)
    {
        if (twentyDollarBills < 0 || fiftyDollarBills < 0 || oneHundredDollarBills < 0)
        {
            throw new ArgumentException("Input values cannot be negative.");
        }

        TwentyDollarBills += twentyDollarBills;
        FiftyDollarBills += fiftyDollarBills;
        OneHundredDollarBills += oneHundredDollarBills;
    }
    //Сделать функцию, снимающую деньги, которая принимает сумму денег, а
    //возвращает булевое значение - успешность выполнения операции.
    //При снятии денег функция должна распечатывать каким количеством
    //купюр какого номинала выдаётся сумма.
    public bool WithdrawCash(int amount)
    {
        if (amount < 0)
        {
            Console.WriteLine("Invalid withdrawal amount.");
            return false;
        }

        if (amount % 10 != 0)
        {
            Console.WriteLine("Withdrawal amount must be a multiple of $10.");
            return false;
        }

        List<int> combinations = new List<int>();
        int[] banknotes = new int[] { 20, 100, 50 };
        int[] counts = new int[banknotes.Length];
        int[] availableCounts = new int[] { TwentyDollarBills, OneHundredDollarBills, FiftyDollarBills };

        if (TryGetCombination(amount, banknotes, counts, availableCounts, combinations))
        {
            for (int i = 0; i < banknotes.Length; i++)
            {
                int count = counts[i];
                if (count > 0)
                {
                    Console.WriteLine($"Withdrawn: ${banknotes[i]}x{count}");
                }
            }

            UpdateAvailableBanknotes(counts);
            return true;
        }

        Console.WriteLine("Insufficient banknotes available for this amount.");
        return false;
    }

    private bool TryGetCombination(int amount, int[] banknotes, int[] counts, int[] availableCounts, List<int> combinations)
    {
        if (amount == 0)
        {
            combinations.AddRange(counts);
            return true;
        }

        for (int i = 0; i < banknotes.Length; i++)
        {
            if (availableCounts[i] > 0 && amount >= banknotes[i])
            {
                availableCounts[i]--;
                counts[i]++;
                combinations.Add(banknotes[i]);

                if (TryGetCombination(amount - banknotes[i], banknotes, counts, availableCounts, combinations))
                    return true;

                availableCounts[i]++;
                counts[i]--;
                combinations.RemoveAt(combinations.Count - 1);
            }
        }

        return false;
    }

    private void UpdateAvailableBanknotes(int[] counts)
    {
        TwentyDollarBills -= counts[0];
        FiftyDollarBills -= counts[1];
        OneHundredDollarBills -= counts[2];
    }
}



}

/*
  public bool WithdrawCash(int amount)
    {
        var totalCashValueInsideATM = TwentyDollarBills * 20 + FiftyDollarBills * 50 + OneHundredDollarBills * 100;
        if (amount <= totalCashValueInsideATM && amount > 0)
        {
            int amountToWithdraw = amount;
            int oneHundredDollarBillsUsed = 0;
            int fiftyDollarBillsUsed = 0;
            int twentyDollarBillsUsed = 0;
            while (amountToWithdraw > 0)
            {
                // If there's at least one hundred-dollar bill, dispense one.
                if (OneHundredDollarBills > 0 && amountToWithdraw >= 100 )
                {
                    OneHundredDollarBills--;
                    oneHundredDollarBillsUsed++;
                    amountToWithdraw -= 100;
                }
                // If there's at least one fifty-dollar bill, dispense one.
                else if (FiftyDollarBills > 0 && amountToWithdraw >= 50)
                {
                    FiftyDollarBills--;
                    amountToWithdraw -= 50;
                    fiftyDollarBillsUsed++;
                }
                // If there's at least one twenty-dollar bill, dispense one.
                else if (TwentyDollarBills > 0 && amountToWithdraw >= 20)
                {
                    TwentyDollarBills--;
                    amountToWithdraw -= 20;
                    twentyDollarBillsUsed++;
                }
                else 
                {
                    Console.WriteLine("Selected cash amount can not be withdrawn due to lack of banknote denominations.");
                    return false;
                }
            }
            Console.WriteLine($"Withdrawal successful. \nCash withdrawn: {amount}" +
                $"\nBanknotes $100 used: {oneHundredDollarBillsUsed} " +
                $"\nBanknotes $50 used: {fiftyDollarBillsUsed} " +
                $"\nBanknotes $20 used: {twentyDollarBillsUsed}" );
            return true;
        }
        else if (amount == 0)
        {
        Console.WriteLine("Please, input a sum for a withdrawl.");
            return false;
        }
        else
        { 
        Console.WriteLine("ATM is out of cash.");
            return false;
        }
    }
*/

