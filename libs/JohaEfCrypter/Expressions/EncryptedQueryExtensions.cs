using System.Linq;

namespace JohaEfCrypter.Expressions
{
    public static class EncryptedQueryExtensions
    {
        public static IQueryable<T> Encrypted<T>(this IQueryable<T> source)
        {
            return new EncryptedQueryable<T>(source);
        }
    }
}
