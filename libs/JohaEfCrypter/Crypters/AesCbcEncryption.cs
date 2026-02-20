using System.IO;
using System.Security.Cryptography;

using JohaEfCrypter.Config;

namespace JohaEfCrypter.Crypters
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

            using MemoryStream ms = new MemoryStream();
            using (var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(plaintextBytes, 0, plaintextBytes.Length);
                cryptoStream.FlushFinalBlock();
            }

            return ms.ToArray();
        }

        // Decrypt
        public static byte[] Decrypt(byte[] ciphertext)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using MemoryStream ms = new MemoryStream();
            using (var cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(ciphertext, 0, ciphertext.Length);
                cryptoStream.FlushFinalBlock();
            }
            return ms.ToArray();
        }
    }
}
