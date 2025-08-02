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
}