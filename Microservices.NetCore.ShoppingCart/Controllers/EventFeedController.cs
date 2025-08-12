using Microservices.NetCore.Shared.EventFeed;

namespace Microservices.NetCore.ShoppingCart.Controllers;

public class EventFeedController(IEventFeed eventFeed) : EventFeedControllerBase(eventFeed);