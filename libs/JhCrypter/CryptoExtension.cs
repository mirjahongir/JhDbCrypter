using System;
using System.Security.Cryptography;
using System.Text;

using JhCrypter.Crypters;

namespace JhCrypter
{
    public static class CryptoExtension
    {
       public const string prefix = "crypt:";
        public const string hash = "hash:";
        public static byte[] ToHash(this string key)
        {
            using var sha = SHA256.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(key));
        }
        public static string HashString(this string key)
        {
            var hash = Convert.ToBase64String(ToHash(key));
            return hash;
           
        }

        #region Encrypt
        public static byte[] Encrypt(this byte[] data) => BaseEncrypter.Encrypt(data);
        public static byte[] Encrypt(this string plainText)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            return Encrypt(data);
        }
        public static string EncryptStr(this string plainText)
        {
            var data = Encrypt(plainText);
            return Convert.ToBase64String(data);
        }
        #endregion

        #region Decrypt
        public static string DecryptBase64(this string base64)
        {
            var data = Convert.FromBase64String(base64);
            var d = Decrypt(data);
            return Encoding.UTF8.GetString(d);
        }
        public static string DecryptString(this string text)
        {
            var data = Encoding.UTF8.GetBytes(text);
            return Encoding.UTF8.GetString(Decrypt(data));
        }

        public static byte[] Decrypt(this byte[] data) => BaseEncrypter.Decrypt(data);
        #endregion

    }
}
