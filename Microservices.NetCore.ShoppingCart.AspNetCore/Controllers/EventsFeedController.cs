using Microservices.NetCore._Shared.EventFeed;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.Controllers;

[ApiController]
[Route("events")]
public class EventsFeedController(IEventFeed eventFeed) : ControllerBase
{
    [HttpGet]
    public ValueTask<IEnumerable<Event>> GetEvents(long start = 0, long end = long.MaxValue)
    {
        return eventFeed.Get(start, end);
    }
}