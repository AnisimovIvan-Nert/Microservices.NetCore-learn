using Microservices.NetCore.Shared.EventFeed;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.Shared.AspNetCore.EventFeed;

[ApiController]
[Route("events")]
public abstract class EventsFeedControllerBase(IEventFeed eventFeed) : ControllerBase
{
    [HttpGet]
    public ValueTask<IEnumerable<Event>> GetEvents(long start = 0, long end = long.MaxValue)
    {
        return eventFeed.Get(start, end);
    }
}