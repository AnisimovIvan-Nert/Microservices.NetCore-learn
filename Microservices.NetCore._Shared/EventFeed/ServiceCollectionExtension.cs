using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.Shared.EventFeed;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddEventFeed<TStore>(this IServiceCollection serviceCollection)
        where TStore : class, IEventStore
    {
        return serviceCollection
            .AddScoped<IEventFeed, Shared.EventFeed.Implementation.EventFeed>()
            .AddScoped<IEventStore, TStore>();
    }
    
    public static IServiceCollection RemoveEventFeed(this IServiceCollection serviceCollection)
    {
        var feedDescriptor = serviceCollection.Single(descriptor => descriptor.ServiceType == typeof(IEventFeed));
        var storeDescriptor = serviceCollection.Single(descriptor => descriptor.ServiceType == typeof(IEventStore));

        serviceCollection.Remove(feedDescriptor);
        serviceCollection.Remove(storeDescriptor);
        return serviceCollection;
    }
}