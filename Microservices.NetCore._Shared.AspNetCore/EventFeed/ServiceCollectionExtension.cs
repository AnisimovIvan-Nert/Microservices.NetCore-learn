using Microservices.NetCore.Shared.EventFeed;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore.Shared.AspNetCore.EventFeed;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddEventFeed(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IEventFeed, Shared.EventFeed.EventFeed>()
            .AddScoped<IEventStore, EventStore>();
    }
}