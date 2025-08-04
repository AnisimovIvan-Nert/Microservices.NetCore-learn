using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.Shared.Cache;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCache(this IServiceCollection serviceCollection, StoreType storeType = StoreType.Default)
    {
        if (storeType == StoreType.Default)
            storeType = StoreType.InMemory;
        
        return storeType switch
        {
            StoreType.InMemory => serviceCollection.AddScoped<ICacheStore, InMemoryCacheStore>(),
            _ => throw new ArgumentOutOfRangeException(nameof(storeType), storeType, null)
        };
    }
    
    public enum StoreType
    {
        Default,
        InMemory
    }
}