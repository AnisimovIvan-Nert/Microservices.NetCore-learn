using Microservices.NetCore.Shared.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.Shared.AspNetCore.Cache;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCache(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<ICacheStore, CacheStore>();
    }
}