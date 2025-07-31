namespace Microservices.NetCore.Shared.EventFeed.Implementation;

public class EventFeedSource : IEventFeedSource
{
    public IEventFeed EventFeed { get; }
}