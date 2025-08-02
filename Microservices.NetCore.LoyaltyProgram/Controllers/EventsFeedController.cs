using Microservices.NetCore.Shared.EventFeed;

namespace Microservices.NetCore.LoyaltyProgram.Controllers;

public class EventsFeedController(IEventFeed eventFeed) : EventsFeedControllerBase(eventFeed);