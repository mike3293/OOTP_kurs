using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WpfClient.Auth
{
    public static class PasswordEncoder
    {
        public static string GetHash(string password, string salt)
        {
            byte[] passwordAndSaltBytes = Concat(password, salt);
            string hash = ComputeHash(passwordAndSaltBytes);

            return hash;
        }

        public static bool Verify(string hash, string salt, string password)
        {
            byte[] passwordAndSaltBytes = Concat(password, salt);
            string hashAttempt = ComputeHash(passwordAndSaltBytes);
            return hash == hashAttempt;
        }

        private static string ComputeHash(byte[] bytes)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(bytes));
            }
        }

        private static byte[] Concat(string password, string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            return passwordBytes.Concat(saltBytes).ToArray();
        }
    }
}
