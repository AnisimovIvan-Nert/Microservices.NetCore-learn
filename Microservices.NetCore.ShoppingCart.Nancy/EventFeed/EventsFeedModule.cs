using Nancy;

namespace Microservices.NetCore.ShoppingCart.Nancy.EventFeed;

public sealed class EventsFeedModule : NancyModule
{
    private const string ModuleUri = "/events";

    public EventsFeedModule(IEventStore eventStore) : base(ModuleUri)
    {
        Get("/", _ =>
        {
            var start = Request.Query.start.Value;
            if (long.TryParse(start, out long startEvent) == false)
                startEvent = 0;

            var end = Request.Query.end.Value;
            if (long.TryParse(end, out long endEvent) == false)
                endEvent = long.MaxValue;

            return eventStore.GetEvents(startEvent, endEvent);
        });
    }
}