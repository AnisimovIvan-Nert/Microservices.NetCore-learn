using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.Shared.Store.InMemory;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInMemoryStoreSource(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IStoreSource, InMemoryStoreSource>();
    }
}