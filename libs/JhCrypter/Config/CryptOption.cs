using JohaEfCrypter.Enums;

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
        
    }
}
