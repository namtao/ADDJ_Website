using System;
using System.Security.Cryptography;
using System.Text;

namespace ADDJ.Sercurity
{
    public class Encrypt
    {
        public static string MD5User(string s)
        {
            string str1 = MD5(s);
            str1 = str1 + s;
            string str2 = MD5(str1);

            return str2;
        }

        public static string MD5Admin(string s)
        {
            return MD5(s);
        }

        public static string MD5(string s)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            // Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = Encoding.Default.GetBytes(s);
            encodedBytes = md5.ComputeHash(originalBytes);

            // Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes).ToLower().Replace("-", "");
        }
    }
}