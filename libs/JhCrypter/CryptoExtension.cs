using JhCrypter.Config;
using JhCrypter.Crypters;
using System;
using System.Text;

namespace JhCrypter
{

    public static class CryptoExtension
    {
        #region Encrypt
        public static byte[] Encrypt(this byte[] data)
        {
            if (Encoding.UTF8.GetString(data.AsSpan()[..Prefix.CryptPrefix.Length]) == Prefix.CryptPrefix)
            {
                return data;
            }
            var encrypt = BaseEncrypter.Encrypt(data);
           // var test= BaseEncrypter.Decrypt(encrypt);
            var result = new byte[encrypt.Length + Prefix.CryptPrefix.Length];
            Array.Copy(Prefix.CryptData, 0, result, 0, Prefix.CryptData.Length);
            Array.Copy(encrypt, 0, result, Prefix.CryptData.Length, encrypt.Length);
            return result;
        }

        public static byte[] Encrypt(this string plainText)
        {
            if (BaseEncrypter.IsBase64(plainText))
            {
                byte[] data = Convert.FromBase64String(plainText);
                return Encrypt(data);
            }
            return Encrypt(Encoding.UTF8.GetBytes(plainText));
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
        //public static string DecryptString(this string text)
        //{
        //    if (BaseEncrypter.IsBase64(text))
        //    {
        //      Convert.  Convert.FromBase64String(text);
        //    }
        //    var data = Encoding.UTF8.GetBytes(text);
        //    return Encoding.UTF8.GetString(Decrypt(data));
        //}

        public static byte[] Decrypt(this byte[] data)
        {
            var span = data.AsSpan();
            if (Encoding.UTF8.GetString(span.Slice(0, Prefix.CryptData.Length)) != Prefix.CryptPrefix)
            {
                throw new Exception("Prexist not exist");
            }
            var dataa = span.Slice(Prefix.CryptData.Length, data.Length- Prefix.CryptData.Length);
            return BaseEncrypter.Decrypt(dataa);
        }
        #endregion

    }
}
