using Microservices.NetCore.Shared.EventFeed;
using Nancy.TinyIoc;

namespace Microservices.NetCore.Shared.Nancy.EventFeed;

public static class TinyIoCContainerExtensions
{
    public static void RegisterEventFeed(this TinyIoCContainer container)
    {
        container.Register<IEventStore, EventStore>();
        container.Register<IEventFeed, Shared.EventFeed.EventFeed>();
    }
}