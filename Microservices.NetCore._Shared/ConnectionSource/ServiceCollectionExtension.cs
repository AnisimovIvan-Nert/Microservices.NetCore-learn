using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.Shared.ConnectionSource;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddConnectionSource<TSource, TTarget, TConnection>(this IServiceCollection serviceCollection)
        where TSource : class, IConnectionSource<TTarget, TConnection>
    {
        return serviceCollection.AddScoped<IConnectionSource<TTarget, TConnection>, TSource>();
    }
    
    public static IServiceCollection RemoveConnectionSource<TTarget, TConnection>(this IServiceCollection serviceCollection)
    {
        var connectionSource = serviceCollection.Single(descriptor => descriptor.ServiceType == typeof(IConnectionSource<TTarget, TConnection>));
        serviceCollection.Remove(connectionSource);
        return serviceCollection;
    }
    
    public static IServiceCollection AddConnectionStringSource<TSource, TTarget>(this IServiceCollection serviceCollection)
        where TSource : class, IConnectionStringSource<TTarget>
    {
        return serviceCollection.AddScoped<IConnectionStringSource<TTarget>, TSource>();
    }
    
    public static IServiceCollection RemoveConnectionStringSource<TTarget>(this IServiceCollection serviceCollection)
    {
        var connectionSource = serviceCollection.Single(descriptor => descriptor.ServiceType == typeof(IConnectionStringSource<TTarget>));
        serviceCollection.Remove(connectionSource);
        return serviceCollection;
    }
}