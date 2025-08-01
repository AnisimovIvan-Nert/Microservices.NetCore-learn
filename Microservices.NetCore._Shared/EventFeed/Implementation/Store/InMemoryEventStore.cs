namespace Microservices.NetCore.Shared.EventFeed.Implementation.Store;

public class InMemoryEventStore : IEventStore
{
    private static long _currentSequenceNumber;
    private static readonly Dictionary<string, List<Event>> _database = [];

    private string _eventType = "default";
    
    public void SetEventType(string eventType)
    {
        _eventType = eventType;
    }

    public ValueTask<IEnumerable<Event>> GetEvents(long firstNumber, long lastNumber)
    {
        var events = _database[_eventType].Where(e => 
                e.SequenceNumber >= firstNumber 
                && e.SequenceNumber <= lastNumber)
            .OrderBy(e => e.SequenceNumber);

        return ValueTask.FromResult(events.AsEnumerable());
    }

    public ValueTask Raise(string eventName, object content)
    {
        var sequenceNumber = Interlocked.Increment(ref _currentSequenceNumber);
        var currentTime = DateTimeOffset.UtcNow;
        var newEvent = new Event(sequenceNumber, currentTime, eventName, content, _eventType);
        if (_database.ContainsKey(_eventType) == false)
            _database[_eventType] = [];
        _database[_eventType].Add(newEvent);
        return ValueTask.CompletedTask;
    }
}