using JhCrypter.Config;

using JohaEfCrypter.Enums;
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
        public static byte[] Decrypt(byte[] encrypt)
        {
            return CryptConfig.EncryptType switch
            {
                EncryptType.AesCbc => AesCbcEncryption.Decrypt(encrypt),
                EncryptType.AesGcm => AesGcmEncrypter.Decrypt(encrypt),
                _ => AesCbcEncryption.Decrypt(encrypt),
            };
        }
    }
}
