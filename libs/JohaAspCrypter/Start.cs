using System;
using System.Reflection;
using System.Threading;

using JohaAspCrypter.HostedServices;

using JohaEfCrypter.Config;
using JohaEfCrypter.Extensions;
using JohaEfCrypter.Intecepters;
using JohaEfCrypter.Interfaces;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JohaAspCrypter
{

    public static class RegisterService
    {
        static IServiceProvider _service;
        public static IServiceCollection RegisterJhCrypter(this IServiceCollection service, Action<CryptOption> action)
        {
            CryptOption option = new()
            {
                EncryptType = JohaEfCrypter.Enums.EncryptType.AesCbc,
                Key = "test"
            };
            action(option);
            InterceptorExtension.ErrorAct += Erroraction;
            CryptConfig.Option = option;
            service.AddScoped<IWorker, UpdateDbHostService>();
            service.TryAddSingleton<CryptoInterceptor>();
            service.AddSingleton<DecryptioInterceptor>();
            return service;
        }

        private static void Erroraction(object obj)
        {
            using var service = _service.CreateScope();

            var provider = service.ServiceProvider;
            Type entityType = obj.GetType(); // yoki menda bor Type

            Type handlerInterface = typeof(IInterceptorErrorHandler<>)
                .MakeGenericType(entityType);
            var errorHandler = provider.GetService(handlerInterface);
            if (errorHandler == null) return;
            var tip = errorHandler.GetType();
            var method = tip.GetMethod("ErrorHandler");
            method?.Invoke(errorHandler, new[] { obj });
        }

        public static IServiceProvider UpdateDb(this IServiceProvider provider)
        {
            try
            {

                _service = provider;
                var worker = provider.CreateScope().ServiceProvider.GetRequiredService<IWorker>();
                var cts = new CancellationTokenSource();
                worker.StartAsync(cts.Token);
                cts.Cancel();
                worker.StopAsync();
                return provider;
            }
            catch (Exception ext)
            {
                Console.WriteLine(ext);
                throw ext;
            }

        }
    }
}
