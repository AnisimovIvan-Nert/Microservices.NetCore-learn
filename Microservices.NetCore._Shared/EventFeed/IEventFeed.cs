namespace Microservices.NetCore._Shared.EventFeed;

public interface IEventFeed
{
    ValueTask<IEnumerable<Event>> Get(long firstEvent, long lastEvent);
    ValueTask Raise(string name, object content);
}