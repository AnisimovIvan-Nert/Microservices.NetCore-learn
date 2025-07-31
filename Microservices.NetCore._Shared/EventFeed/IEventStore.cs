using Microservices.NetCore.Shared.EventFeed.Implementation;

namespace Microservices.NetCore.Shared.EventFeed;

public interface IEventStore
{
    internal void SetEventType(string eventType);
    ValueTask<IEnumerable<Event>> GetEvents(long firstNumber, long lastNumber);
    ValueTask Raise(string eventName, object content);
}