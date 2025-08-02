using Microservices.NetCore.Shared.EventFeed;

namespace Microservices.NetCore.ShoppingCart.Controllers;

public class EventsFeedController(IEventFeed eventFeed) : EventsFeedControllerBase(eventFeed);