using Microsoft.Extensions.DependencyInjection;

namespace Search.Common.DI
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterModule<TModule>(this IServiceCollection serviceCollection)
            where TModule: class, IModule, new()
        {
            var module = new TModule();
            module.Register(serviceCollection);
        }
    }
}
