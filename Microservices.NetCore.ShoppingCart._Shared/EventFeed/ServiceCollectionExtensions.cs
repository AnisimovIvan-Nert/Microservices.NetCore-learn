using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.ShoppingCart._Shared.EventFeed;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventFeed(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IEventStore, EventStore>()
            .AddScoped<IEventFeed, EventFeed>();
    }
}