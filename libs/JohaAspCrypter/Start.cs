using System;
using System.Threading;
using JhCrypter.Config;
using JohaAspCrypter.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            service.TryAddSingleton<IWorker, UpdateDbHostService>();
            return service;
        }
        public static IServiceProvider UpdateDb(this IServiceProvider provider)
        {
            try
            {
                var worker = provider.GetRequiredService<IWorker>();
                var cts = new CancellationTokenSource();
                worker.StartAsync(cts.Token);
                cts.Cancel();
                worker.StopAsync();
                return provider;
            }
            catch(Exception ext)
            {
                Console.WriteLine("error");
                return null;
            }
            
        }


    }
}
