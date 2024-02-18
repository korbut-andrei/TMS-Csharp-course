using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_Exceptions
{
    public class WrongLoginException : Exception
    {
        public string Username { get; }

        public WrongLoginException(string message, string username) : base(message)
        {
            Username = username;
        }

        public WrongLoginException(string username) : base("Username doesn't match the requirements")
        {
            Username = username;
        }
    }
}
