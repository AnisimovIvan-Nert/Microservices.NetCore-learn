using System.Text.Json;
using Microservices.NetCore.Shared.Store;
using Microservices.NetCore.Shared.ValueGenerators;

namespace Microservices.NetCore.Shared.EventFeed.Implementation.Store;

public class InMemoryEventStore : IEventStore
{
    private readonly IStore<string, List<Event>> _store;
    private readonly IValueGenerator<long> _sequenceGenerator;
    private string _streamName = "default";

    public InMemoryEventStore(IStoreSource storeSource)
    {
        _store = storeSource.GetStore<string, List<Event>>(nameof(InMemoryEventStore));
        var generators = storeSource.GetOrLinkGenerators(_store, CreateGenerators);
        _sequenceGenerator = (IValueGenerator<long>)generators.Single();
        return;

        IReadOnlyCollection<IValueGenerator> CreateGenerators()
        {
            return [new LongGenerator()];
        }
    }

    public void Clear()
    {
        _store.Clear();
        _sequenceGenerator.Reset();
    }
    
    public string GetCurrentStoreStream() => _streamName;
    
    public void SetStoreStream(string streamName)
    {
        _streamName = streamName;
    }

    public ValueTask<IEnumerable<Event>> GetEvents(long firstNumber, long lastNumber)
    {
        var eventCollection = _store.Get(_streamName);
        if (eventCollection == null)
            return ValueTask.FromResult(Enumerable.Empty<Event>());
        
        var events = eventCollection.Where(e => 
                e.SequenceNumber >= firstNumber 
                && e.SequenceNumber <= lastNumber)
            .OrderBy(e => e.SequenceNumber);

        return ValueTask.FromResult(events.AsEnumerable());
    }

    public ValueTask Raise(string eventName, object content)
    {
        var sequenceNumber = _sequenceGenerator.GenerateNext();
        var currentTime = DateTimeOffset.UtcNow;
        var contentJson = JsonSerializer.Serialize(content);
        var newEvent = new Event(sequenceNumber, currentTime, eventName, contentJson, _streamName);
        
        var eventCollection = _store.Get(_streamName);
        if (eventCollection == null)
        {
            eventCollection = [];
            _store.Add(_streamName, eventCollection);
        }
        eventCollection.Add(newEvent);
        return ValueTask.CompletedTask;
    }
}