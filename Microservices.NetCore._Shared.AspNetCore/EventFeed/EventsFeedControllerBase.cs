using Microservices.NetCore._Shared.EventFeed;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore._Shared.AspNetCore.EventFeed;

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