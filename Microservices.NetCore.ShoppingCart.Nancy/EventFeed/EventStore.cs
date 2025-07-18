namespace Microservices.NetCore.ShoppingCart.Nancy.EventFeed;

public class EventStore : IEventStore
{
    private static long _currentSequenceNumber;
    private static readonly IList<Event> _database = new List<Event>();

    public IEnumerable<Event> GetEvents(long firstEvent, long lastEvent)
    {
        return _database.Where(e =>
                e.SequenceNumber >= firstEvent
                && e.SequenceNumber <= lastEvent)
            .OrderBy(e => e.SequenceNumber);
    }

    public void Raise(string eventName, object content)
    {
        var sequenceNumber = Interlocked.Increment(ref _currentSequenceNumber);
        var currentTime = DateTimeOffset.UtcNow;
        var newEvent = new Event(sequenceNumber, currentTime, eventName, content);
        _database.Add(newEvent);
    }
}