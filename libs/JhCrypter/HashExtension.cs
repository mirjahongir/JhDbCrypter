using JhCrypter.Config;
using JhCrypter.Crypters;
using System;
using System.Security.Cryptography;
using System.Text;

namespace JhCrypter
{
    public static class HashExtension
    {
        #region Hash
        static byte[] ToHash(this byte[] data, bool checkPrefix)
        {
            if (Encoding.ASCII.GetString(data.AsSpan()[..Prefix.HeshPrefix.Length]) == Prefix.HeshPrefix)
            {
                return data;
            }
            using var sha = SHA256.Create();
            var hashData = sha.ComputeHash(data);
            if (!checkPrefix)
            {
                return hashData;
            }
            byte[] result = new byte[hashData.Length + Prefix.HeshPrefixdata.Length];
            Array.Copy(Prefix.HeshPrefixdata, 0, result, 0, Prefix.HeshPrefixdata.Length);
            Array.Copy(hashData, 0, result, Prefix.HeshPrefixdata.Length, hashData.Length);
            return result;

        }
        public static byte[] ToHash(this string key, bool checkPrefix = true)
        {
            if (BaseEncrypter.IsBase64(key))
            {
                var item = Convert.FromBase64String(key);
                return ToHash(item,checkPrefix);
            }
            else
            {
                var item1 = Encoding.UTF8.GetBytes(key);
                return ToHash(item1, checkPrefix);
            }
        }
        public static string HashString(this string key, bool checkPrefix = true)
        {
            var hash = Convert.ToBase64String(ToHash(key, checkPrefix));
            return hash;
        }
        #endregion
    }
}
