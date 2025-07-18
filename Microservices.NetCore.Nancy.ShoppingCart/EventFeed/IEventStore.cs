namespace Microservices.NetCore.Nancy.ShoppingCart.EventFeed;

public interface IEventStore
{
    IEnumerable<Event> GetEvents(long firstEvent, long lastEvent);
    void Raise(string eventName, object content);
}