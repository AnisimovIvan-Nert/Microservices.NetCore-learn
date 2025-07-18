namespace Microservices.NetCore.ShoppingCart._Shared.EventFeed;

public interface IEventFeed
{
    IEnumerable<Event> Get(long firstEvent, long lastEvent);
    void Raise(string name, object content);
}