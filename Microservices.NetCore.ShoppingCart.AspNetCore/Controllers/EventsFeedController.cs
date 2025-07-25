using Microservices.NetCore.Shared.AspNetCore.EventFeed;
using Microservices.NetCore.Shared.EventFeed;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.Controllers;

public class EventsFeedController(IEventFeed eventFeed) : EventsFeedControllerBase(eventFeed);