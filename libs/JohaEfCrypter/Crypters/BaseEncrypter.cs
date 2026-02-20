using JohaEfCrypter.Config;

namespace JohaEfCrypter.Crypters
{
    static class BaseEncrypter
    {
        public static byte[] Encrypt(byte[] byteData)
        {
            return CryptConfig.EncryptType switch
            {
                Enums.EncryptType.AesCbc => AesCbcEncryption.Encrypt(byteData),
                Enums.EncryptType.AesGcm => AesGcmEncrypter.Encrypt(byteData),
                _ => AesCbcEncryption.Encrypt(byteData),
            };
        }
        public static byte[] Decrypt(byte[] encrypt)
        {
            return CryptConfig.EncryptType switch
            {
                Enums.EncryptType.AesCbc => AesCbcEncryption.Decrypt(encrypt),
                Enums.EncryptType.AesGcm => AesGcmEncrypter.Decrypt(encrypt),
                _ => AesCbcEncryption.Decrypt(encrypt),
            };
        }
    }
}
