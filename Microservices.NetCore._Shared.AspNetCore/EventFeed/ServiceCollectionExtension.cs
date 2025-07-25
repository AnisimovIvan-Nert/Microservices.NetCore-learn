using Microservices.NetCore._Shared.EventFeed;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.NetCore._Shared.AspNetCore.EventFeed;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddEventFeed(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IEventFeed, _Shared.EventFeed.EventFeed>()
            .AddScoped<IEventStore, EventStore>();
    }
}