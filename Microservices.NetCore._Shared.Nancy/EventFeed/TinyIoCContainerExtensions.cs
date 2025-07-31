using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Shared.EventFeed.Implementation.Store;
using Nancy.TinyIoc;

namespace Microservices.NetCore.Shared.Nancy.EventFeed;

public static class TinyIoCContainerExtensions
{
    public static void RegisterEventFeed(this TinyIoCContainer container)
    {
        container.Register<IEventStore, InMemoryEventStore>();
        container.Register<IEventFeed, Shared.EventFeed.Implementation.EventFeed>();
    }
}