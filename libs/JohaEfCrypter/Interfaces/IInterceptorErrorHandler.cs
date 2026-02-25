using System.Threading.Tasks;

namespace JohaEfCrypter.Interfaces
{
    public interface IInterceptorErrorHandler<T>
    {
        ValueTask ErrorHandler(T obj);
    }
}
