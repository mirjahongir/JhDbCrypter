using JohaEfCrypter.Enums;

namespace JhCrypter.Config
{
    public static class CryptConfig
    {
        public static CryptOption? Option;
        internal static EncryptType EncryptType => Option?.EncryptType ?? EncryptType.AesCbc;
        internal static byte[]? keyData { get; private set; }
        internal static byte[] AesKey
        {
            get
            {
                if (string.IsNullOrEmpty(Option?.Key))
                {

                    throw new System.Exception("Key not found");
                }
                if (keyData != null && keyData.Length == 32) return keyData;
                keyData = CryptoExtension.ToHash(Option.Key);
                return keyData;
            }
        }
    }
}
