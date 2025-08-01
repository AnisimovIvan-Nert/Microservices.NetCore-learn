using Microservices.NetCore.Shared.Cache;
using Nancy.TinyIoc;

namespace Microservices.NetCore.Shared.Nancy.Cache;

public static class TinyIoCContainerExtensions
{
    public static void RegisterEventFeed(this TinyIoCContainer container)
    {
        container.Register<ICacheStore, CacheStore>();
    }
}