using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Search.Common.DI;
using SearchFight.Console.Application;
using SearchFight.Services;

namespace SearchFight.Console.DI
{
    public static class Bootstrapper
    {
        public static IServiceProvider Start()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.ConfigureLogging())
                .ConfigureServiceCollection()
                .BuildServiceProvider(new ServiceProviderOptions
                {
                    ValidateOnBuild = true, 
#if DEBUG
                    ValidateScopes = true
#endif
                });

            return serviceProvider;
        }

        private static IServiceCollection ConfigureServiceCollection(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IApplication, Application.Application>();
            serviceCollection.RegisterModule<CustomSearchModule>();
            return serviceCollection;
        }

        private static void ConfigureLogging(this ILoggingBuilder loggingBuilder)
        {
#if DEBUG
            loggingBuilder.AddDebug().SetMinimumLevel(LogLevel.Debug);
#endif
        }
    }
}
