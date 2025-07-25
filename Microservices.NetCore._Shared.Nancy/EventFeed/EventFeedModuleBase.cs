using Microservices.NetCore.Shared.EventFeed;
using Nancy;

namespace Microservices.NetCore.Shared.Nancy.EventFeed;

public abstract class EventsFeedModuleBase : NancyModule
{
    private readonly IEventFeed _eventFeed;
    
    protected EventsFeedModuleBase(IEventFeed eventFeed, string moduleUri) : base(moduleUri)
    {
        _eventFeed = eventFeed;
        SetUpRoutes();
    }

    protected virtual void SetUpRoutes()
    {
        Get("/", async _ =>
        {
            long startEvent = 0;
            var start = Request.Query.start.Value;
            if (start != null)
                long.TryParse(start, out startEvent);

            var endEvent = long.MaxValue;
            var end = Request.Query.end.Value;
            if (end != null)
                long.TryParse(end, out endEvent);

            return await _eventFeed.Get(startEvent, endEvent);
        });
    }
}