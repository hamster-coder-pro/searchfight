using Microsoft.Extensions.DependencyInjection;

namespace Search.Common.DI
{
    public interface IModule
    {
        void Register(IServiceCollection serviceCollection);
    }
}