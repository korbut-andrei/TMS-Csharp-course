using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_Exceptions
{
    public class WrongPasswordException : Exception
    {
        public string Password { get; }

        public WrongPasswordException(string message, string password) : base(message)
        {
            Password = password;
        }

        public WrongPasswordException(string password) : base("Password doesn't match the requirements")
        {
            Password = password;
        }
    }
}
