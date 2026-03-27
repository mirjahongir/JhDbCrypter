using JhCrypter.Config;
using JohaEfCrypter.Enums;
using System;
namespace JhCrypter.Crypters
{
    static class BaseEncrypter
    {
        public static byte[] Encrypt(byte[] byteData)
        {
            return CryptConfig.EncryptType switch
            {
                EncryptType.AesCbc => AesCbcEncryption.Encrypt(byteData),
                EncryptType.AesGcm => AesGcmEncrypter.Encrypt(byteData),
                _ => AesCbcEncryption.Encrypt(byteData),
            };
        }
        public static byte[] Decrypt(in ReadOnlySpan< byte> encrypt)
        {
            return CryptConfig.EncryptType switch
            {
                EncryptType.AesCbc => AesCbcEncryption.Decrypt(encrypt),
                EncryptType.AesGcm => AesGcmEncrypter.Decrypt(encrypt),
                _ => AesCbcEncryption.Decrypt(encrypt),
            };
        }

        public static bool IsBase64(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();

            // uzunligi 4 ga karrali bo‘lishi kerak
            if (input.Length % 4 != 0)
                return false;

            Span<byte> buffer = new Span<byte>(new byte[input.Length]);

            return Convert.TryFromBase64String(input, buffer, out _);
        }
    }
}
