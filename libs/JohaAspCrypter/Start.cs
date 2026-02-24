using System;
using System.Threading;

using JohaAspCrypter.HostedServices;

using JohaEfCrypter.Config;
using JohaEfCrypter.Intecepters;

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
            service.AddScoped<IWorker, UpdateDbHostService>();
            service.AddSingleton<CryptoInterceptor>();
            service.AddSingleton<DecryptioInterceptor>();
            return service;
        }
        public static IServiceProvider UpdateDb(this IServiceProvider provider)
        {
            try
            {

                var worker = provider.CreateScope().ServiceProvider.GetRequiredService<IWorker>();// provider.GetRequiredService<IWorker>();
                var cts = new CancellationTokenSource();
                worker.StartAsync(cts.Token);
                cts.Cancel();
                worker.StopAsync();
                return provider;
            }
            catch (Exception ext)
            {
                Console.WriteLine("error");
                return null;
            }

        }


    }
}
