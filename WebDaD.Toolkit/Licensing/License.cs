using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace WebDaD.Toolkit.Licensing
{
    public static class License
    {
        private static readonly string SECRET = "57f18dc5b38ad2f414c6e19229a20af8";

        public static bool Check(string username, string key)
        {
            string t_key = "";

            t_key = username + SECRET;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(t_key);
            byte[] result = md5.ComputeHash(textToHash);

            return System.BitConverter.ToString(result).Equals(key);
        }

        public static string GenerateKey(string username)
        {
            string key = "";

            key = username + SECRET;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(key);
            byte[] result = md5.ComputeHash(textToHash);

            return System.BitConverter.ToString(result);
        }
    }
}
