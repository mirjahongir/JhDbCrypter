using System;
using JohaEfCrypter.Config;
using Microsoft.Extensions.DependencyInjection;

namespace JohaAspCrypter
{
    public static class RegisterService
    {
        public static IServiceCollection RegisterJhCrypter(this IServiceCollection service, Action<CryptOption> action)
        {
            CryptOption option = new()
            {
                EncryptType = JohaEfCrypter.Enums.EncryptType.AesCbc,
                Key = "test"
            };
            action(option);
            CryptConfig.Option = option;
            return service;
        }


    }
}
