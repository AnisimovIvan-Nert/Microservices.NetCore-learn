using Microservices.NetCore.Shared.EventFeed;

namespace Microservices.NetCore.LoyaltyProgram.Controllers;

public class EventFeedController(IEventFeed eventFeed) : EventFeedControllerBase(eventFeed);