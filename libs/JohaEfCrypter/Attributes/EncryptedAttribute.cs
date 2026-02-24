using System;
using System.Linq;

namespace JohaEfCrypter.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptedAttribute : Attribute
    {
        public EncryptedAttribute() { }
        public bool IsHash { get; set; } = false;
        public string? HashField { get; set; }
        public bool CheckSum { get; set; } = false;
        public bool IsEncrypt { get; set; } = false;
        public static void Validate(Type type)
        {
            var invalidProps = type
                .GetProperties()
                .Where(p => p.IsDefined(typeof(EncryptedAttribute), false)
                         && p.PropertyType != typeof(string))
                .ToList();

            if (invalidProps.Any())
            {
                throw new InvalidOperationException(
                    $"[Encrypted] faqat string property uchun ishlatiladi. Xato: {string.Join(", ", invalidProps.Select(p => p.Name))}");
            }
        }
    }
}
