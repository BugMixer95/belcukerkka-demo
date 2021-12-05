using System.Security.Cryptography;
using System.Text;

namespace Belcukerkka.Security
{
    public static class PasswordHandler
    {
        /// <summary>
        /// String that will be added to a password while hashing.
        /// </summary>
        private static string Salt { get; } = "bC-";

        /// <summary>
        /// Creates MD5'ed password as string.
        /// </summary>
        /// <param name="password">Password that should be hashed.</param>
        /// <returns>Specified hash set of specified password.</returns>
        public static string HashPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                string saltedPassword = string.Concat(Salt, password);

                byte[] inputBytes = new UTF8Encoding().GetBytes(saltedPassword);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
