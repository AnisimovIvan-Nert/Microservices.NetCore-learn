using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Shared.Nancy.EventFeed;

namespace Microservices.NetCore.ShoppingCart.Nancy.Modules;

public sealed class EventsFeedModule(IEventFeed eventFeed) 
    : EventsFeedModuleBase(eventFeed, ModuleUri)
{
    private const string ModuleUri = "/events";
}