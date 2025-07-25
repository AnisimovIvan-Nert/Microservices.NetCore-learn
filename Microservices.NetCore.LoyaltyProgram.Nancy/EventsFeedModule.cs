using Microservices.NetCore._Shared.EventFeed;
using Microservices.NetCore.Shared.Nancy.EventFeed;

namespace Microservices.NetCore.LoyaltyProgram.Nancy;

public sealed class EventsFeedModule(IEventFeed eventFeed) 
    : EventsFeedModuleBase(eventFeed, ModuleUri)
{
    private const string ModuleUri = "/events";
}