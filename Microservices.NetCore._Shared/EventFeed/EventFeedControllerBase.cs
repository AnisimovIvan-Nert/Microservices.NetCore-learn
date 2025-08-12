using Microservices.NetCore.Shared.EventFeed.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.Shared.EventFeed;

[ApiController]
[Route("events")]
public abstract class EventFeedControllerBase(IEventFeed eventFeed) : ControllerBase
{
    [HttpGet]
    public ValueTask<IEnumerable<Event>> GetEvents(long start = 0, long end = long.MaxValue)
    {
        return eventFeed.Get(start, end);
    }
}