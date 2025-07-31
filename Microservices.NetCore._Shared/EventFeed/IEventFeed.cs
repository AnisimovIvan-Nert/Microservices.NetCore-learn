using Microservices.NetCore.Shared.EventFeed.Implementation;

namespace Microservices.NetCore.Shared.EventFeed;

public interface IEventFeed
{
    internal void SetEventType(string eventType);
    ValueTask<IEnumerable<Event>> Get(long firstEvent, long lastEvent);
    ValueTask Raise(string name, object content);
}