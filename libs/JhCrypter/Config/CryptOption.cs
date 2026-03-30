using JohaEfCrypter.Enums;
using System.Text;

namespace JhCrypter.Config
{
    public class CryptOption
    {
        public EncryptType EncryptType { get; set; }

        public string? Key { get; set; }
        public string? SigningKey { get; set; }
        public string? AuthEncryptingKey { get; set; }
    }
    public static class Prefix
    {
        public const string CryptPrefix = "crypt:";
        public const string HeshPrefix = "hash:";

        static byte[]? privateHashData = null;
        public static byte[] HeshPrefixdata
        {
            get
            {
                privateHashData ??= Encoding.UTF8.GetBytes(HeshPrefix);
                return privateHashData;
            }
        }
        static byte[]? privCryptData = null;
        public static byte[] CryptData
        {
            get
            {
                privCryptData ??= Encoding.ASCII.GetBytes(CryptPrefix);
                return privCryptData;
            }
        }

    }
}
