using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_Exceptions
{
    public static class Authenticate
    {
        private const int MaxPasswordLength = 20;
        private const int MaxLoginLength = 20;

        public static bool LogIn(string username, string password, string passwordConfirm)
        {
           try
            {
                ValidatePassword(password);
                ValidateUsername(username);
                ComparePasswords(password, passwordConfirm);
                Console.WriteLine("Login successful.");
                return true;
            }
            catch (WrongLoginException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (WrongPasswordException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private static void ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new WrongLoginException("Username was empty", username);
            }

            if (username.Length > MaxLoginLength)
            {
                throw new WrongLoginException($"Username length was greater than {MaxLoginLength}", username);
            }

            if (username.Contains(" "))
            {
                throw new WrongLoginException("Username contains white spaces.", username);
            }
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new WrongPasswordException("Password is empty.", password);
            }

            if (password.Length > MaxLoginLength)
            {
                throw new WrongPasswordException($"Password length was greater than {MaxPasswordLength}", password);
            }

            if ( password.Contains(" "))
            {
                throw new WrongPasswordException("Password contains white spaces.");
            }

            if (!password.Any(char.IsDigit))
            {
                throw new WrongPasswordException("Password must include at least one digit.");
            }
        }

        private static bool ComparePasswords(string inputPassword, string storedPassword)
        {
            if(inputPassword != storedPassword)
            {
                throw new WrongPasswordException("Password must include at least one digit.");
            }
            return true;

        }
    }
}
