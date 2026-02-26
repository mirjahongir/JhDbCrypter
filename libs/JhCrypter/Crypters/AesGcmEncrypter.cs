using System;
using System.Linq;
using System.Security.Cryptography;

using JhCrypter.Config;

namespace JhCrypter.Crypters
{

     static class AesGcmEncrypter
    {
        //// 256-bit kalit (32 byte)
        //public static readonly byte[] Key = Convert.FromBase64String("YOUR_BASE64_32_BYTE_KEY_HERE");

        // Ma'lumotni shifrlash
        public static byte[] Encrypt(byte[] plaintextBytes)
        {
            // GCM nonce 12 byte
            byte[] nonce = new byte[12];
            RandomNumberGenerator.Fill(nonce);

            byte[] ciphertext = new byte[plaintextBytes.Length];
            byte[] tag = new byte[16]; // 128-bit auth tag

            using var aes = new AesGcm(CryptConfig.AesKey);
            aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);

            // DB uchun: nonce + tag + ciphertext birlashtiriladi
            return Combine(nonce, tag, ciphertext);
        }

        // Ma'lumotni decrypt qilish
        public static byte[] Decrypt(byte[] encryptedData)
        {
            byte[] nonce = new byte[12];
            byte[] tag = new byte[16];
            byte[] ciphertext = new byte[encryptedData.Length - 12 - 16];

            Array.Copy(encryptedData, 0, nonce, 0, 12);
            Array.Copy(encryptedData, 12, tag, 0, 16);
            Array.Copy(encryptedData, 28, ciphertext, 0, ciphertext.Length);

            byte[] decrypted = new byte[ciphertext.Length];

            using var aes = new AesGcm(CryptConfig.AesKey);
            aes.Decrypt(nonce, ciphertext, tag, decrypted);

            return decrypted;
        }

        // Byte array larni birlashtirish yordamchi
        private static byte[] Combine(params byte[][] arrays)
        {
            byte[] combined = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (var array in arrays)
            {
                Buffer.BlockCopy(array, 0, combined, offset, array.Length);
                offset += array.Length;
            }
            return combined;
        }

    }
}
