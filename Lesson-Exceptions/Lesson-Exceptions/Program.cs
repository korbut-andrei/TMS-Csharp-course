using Lesson_Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_Exceptions
{
    internal class Program
    {
            static void Main()
            {

            ExecuteLogin("", "", "");
            ExecuteLogin("us", "skdfh", "skdfhhnb");
            ExecuteLogin("us", "skdfhggg", "skdfhggg");
            ExecuteLogin("us555", "skdfhggg", "skdfhggg0");
            ExecuteLogin("us555", "skdfhggg0", "skdfhggg");
            ExecuteLogin("us555", "skdfhggg0", "skdfhggg0");
            ExecuteLogin("us555", "skdfhggg", "skdfhggg");
            ExecuteLogin("Яus555", "skdfhggg0", "skdfhggg0");
            ExecuteLogin("us555", "skdfhggg0Я", "skdfhggg0Я");
            Console.ReadLine();
            void ExecuteLogin(string username, string password, string passwordConfirm)
            {
                Console.WriteLine($"\"{username}\" | \"{password}\" | \"{passwordConfirm}\"");

                try
                {
                    var success = Authenticate.LogIn(username, password, passwordConfirm);
                    Console.WriteLine(success);
                }
                catch (WrongLoginException ex)
                {
                    Console.WriteLine($"{nameof(WrongLoginException)}: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);

                }
                catch (WrongPasswordException ex)
                {
                    Console.WriteLine($"{nameof(WrongPasswordException)}: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
    }
}


