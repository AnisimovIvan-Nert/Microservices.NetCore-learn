using Microservices.NetCore._Shared.AspNetCore.EventFeed;
using Microservices.NetCore._Shared.EventFeed;

namespace Microservices.NetCore.LoyaltyProgram.AspNetCore.Controllers;

public class EventsFeedController(IEventFeed eventFeed) : EventsFeedControllerBase(eventFeed);