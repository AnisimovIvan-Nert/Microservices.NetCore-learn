using Microservices.NetCore.ShoppingCart._Shared.EventFeed;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.ShoppingCart.AspNetCore.Controllers;

[ApiController]
[Route("events")]
public class EventsFeedController(IEventStore eventStore) : ControllerBase
{
    [HttpGet]
    public IEnumerable<Event> GetEvents(long start = 0, long end = long.MaxValue)
    {
        return eventStore.GetEvents(start, end);
    }
}