namespace Microservices.NetCore.Shared.EventFeed.Implementation;

public class EventFeed(IEventStore eventStore) : IEventFeed
{
    public void SetEventType(string eventType)
    {
        eventStore.SetEventType(eventType);
    }

    public ValueTask<IEnumerable<Event>> Get(long firstEvent, long lastEvent)
    { 
        return eventStore.GetEvents(firstEvent, lastEvent);
    }
    
    public ValueTask Raise(string name, object content)
    {
        return eventStore.Raise(name, content);
    }
}