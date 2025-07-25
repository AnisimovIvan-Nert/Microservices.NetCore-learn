namespace Microservices.NetCore._Shared.EventFeed;

public class EventStore : IEventStore
{
    private static long _currentSequenceNumber;
    private static readonly List<Event> _database = [];

    public ValueTask<IEnumerable<Event>> GetEvents(long firstEvent, long lastEvent)
    {
        var events = _database.Where(e => 
                e.SequenceNumber >= firstEvent 
                && e.SequenceNumber <= lastEvent)
            .OrderBy(e => e.SequenceNumber);

        return ValueTask.FromResult(events.AsEnumerable());
    }

    public ValueTask Raise(string eventName, object content)
    {
        var sequenceNumber = Interlocked.Increment(ref _currentSequenceNumber);
        var currentTime = DateTimeOffset.UtcNow;
        var newEvent = new Event(sequenceNumber, currentTime, eventName, content);
        _database.Add(newEvent);
        return ValueTask.CompletedTask;
    }
}