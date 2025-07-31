namespace Microservices.NetCore.Shared.EventFeed;

public interface IEventFeedSource
{
    IEventFeed EventFeed { get; }
}