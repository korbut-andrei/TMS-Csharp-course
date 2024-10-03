using System.Security.Cryptography;
using System.Text;

namespace Final_project.Services
{
    public class HashHelper : IHashHelper
    {
        public int ComputeHash<T>(T input)
        {
            if (input == null)
                return 0;

            // Convert input to a string representation
            string inputString = input.ToString();

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(inputString);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // To ensure consistent hash value and avoid potential issues with BitConverter
                // Ensure that we use a fixed length (e.g., first 4 bytes of the hash)
                return BitConverter.ToInt32(hashBytes, 0);
            }
        }
    }
}
