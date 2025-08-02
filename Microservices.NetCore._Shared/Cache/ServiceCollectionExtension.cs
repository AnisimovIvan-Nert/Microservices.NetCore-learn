using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.Shared.Cache;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCache(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<ICacheStore, InMemoryCacheStore>();
    }
}