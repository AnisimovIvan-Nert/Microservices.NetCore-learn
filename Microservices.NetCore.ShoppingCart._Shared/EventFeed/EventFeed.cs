namespace Microservices.NetCore.ShoppingCart._Shared.EventFeed;

public class EventFeed(IEventStore eventStore) : IEventFeed
{
    public IEnumerable<Event> Get(long firstEvent, long lastEvent)
    {
        return eventStore.GetEvents(firstEvent, lastEvent);
    }
    
    public void Raise(string name, object content)
    {
        eventStore.Raise(name, content);
    }
}