using System;
using System.IO;
using System.Security.Cryptography;
using JhCrypter.Config;

namespace JhCrypter.Crypters
{
    static class AesCbcEncryption
    {
        // 256-bit kalit (32 byte)
        public static byte[] Key => CryptConfig.AesKey;

        // 16-byte IV (doimiy, deterministic uchun)
        // Xavfsizligi past, pattern leak bo'lishi mumkin
        public static readonly byte[] IV = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16];
        //public static readonly byte[] IV =// new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        // Encrypt
        public static byte[] Encrypt(byte[] plaintextBytes)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

            using MemoryStream ms = new();
            using (var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(plaintextBytes, 0, plaintextBytes.Length);
                cryptoStream.FlushFinalBlock();
            }

            return ms.ToArray();
        }

        // Decrypt

        public static byte[] Decrypt(in ReadOnlySpan<byte> ciphertext)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(ciphertext.ToArray());
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var result = new MemoryStream();

            cs.CopyTo(result);
            return result.ToArray();

            //using MemoryStream ms = new(ciphertext);
            //using (var cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            //{
            //    cryptoStream.Write(ciphertext, 0, ciphertext.Length);
            //    cryptoStream.FlushFinalBlock();
            //}
            //return ms.ToArray();
        }
        //public static byte[] Decrypt(in ReadOnlySpan<byte> data)
        //{
        //    if (data.Length < 16)
        //        throw new ArgumentException("Invalid data");

        //    byte[] iv = data.Slice(0, 16).ToArray();
        //    byte[] ciphertext = data.Slice(16).ToArray();

        //    using Aes aes = Aes.Create();
        //    aes.Key = Key;
        //    aes.IV = iv;

        //    using var decryptor = aes.CreateDecryptor();
        //    using var ms = new MemoryStream(ciphertext);
        //    using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        //    using var result = new MemoryStream();

        //    cs.CopyTo(result);
        //    return result.ToArray();
        //}
        //public static byte[] Decrypt(ReadOnlySpan<byte> data)
        //{
        //    if (data.Length < 16)
        //        throw new ArgumentException("Invalid data");

        //    byte[] iv = data.Slice(0, 16).ToArray();
        //    byte[] ciphertext = data.Slice(16).ToArray();

        //    using Aes aes = Aes.Create();
        //    aes.Key = Key;
        //    aes.IV = iv;
        //    aes.Mode = CipherMode.CBC;
        //    aes.Padding = PaddingMode.PKCS7;

        //    using MemoryStream ms = new();
        //    using var cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

        //    cryptoStream.Write(ciphertext);
        //    cryptoStream.FlushFinalBlock();

        //    return ms.ToArray();
        //}
    }
}
