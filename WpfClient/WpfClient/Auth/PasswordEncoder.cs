using System;
using System.Security.Cryptography;
using System.Text;

namespace WpfClient.Auth
{
    internal class PasswordEncoder
    {
        public static string GetHash(string password)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = Encoding.UTF8.GetBytes(password);
                byte[] generatedHash = sha1.ComputeHash(hash);
                string generatedHashString = Convert.ToBase64String(generatedHash);

                return generatedHashString;
            }
        }
    }
}
