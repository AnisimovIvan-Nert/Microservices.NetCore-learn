using System.Text.Json;

namespace Microservices.NetCore.Shared.EventFeed.Implementation.Store;

public class InMemoryEventStore : IEventStore
{
    private static long _currentSequenceNumber = -1;
    private static readonly Dictionary<string, List<Event>> Database = [];

    private string _streamName = "default";

    public static void Clear()
    {
        _currentSequenceNumber = -1;
        Database.Clear();
    }
    
    public string GetCurrentStoreStream() => _streamName;
    
    public void SetStoreStream(string streamName)
    {
        _streamName = streamName;
    }

    public ValueTask<IEnumerable<Event>> GetEvents(long firstNumber, long lastNumber)
    {
        if (Database.TryGetValue(_streamName, out var value) == false)
            return ValueTask.FromResult(Enumerable.Empty<Event>());
        
        var events = value.Where(e => 
                e.SequenceNumber >= firstNumber 
                && e.SequenceNumber <= lastNumber)
            .OrderBy(e => e.SequenceNumber);

        return ValueTask.FromResult(events.AsEnumerable());
    }

    public ValueTask Raise(string eventName, object content)
    {
        var sequenceNumber = Interlocked.Increment(ref _currentSequenceNumber);
        var currentTime = DateTimeOffset.UtcNow;
        var contentJson = JsonSerializer.Serialize(content);
        var newEvent = new Event(sequenceNumber, currentTime, eventName, contentJson, _streamName);
        
        if (Database.ContainsKey(_streamName) == false)
            Database[_streamName] = [];
        Database[_streamName].Add(newEvent);
        return ValueTask.CompletedTask;
    }
}