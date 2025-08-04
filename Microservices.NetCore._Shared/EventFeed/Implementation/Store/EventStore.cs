using System.Text;
using System.Text.Json;
using EventStore.Client;
using Microservices.NetCore.Shared.ConnectionSource;

namespace Microservices.NetCore.Shared.EventFeed.Implementation.Store;

public class EventStore(IConnectionStringSource<IEventStore> connectionSource) : IEventStore
{
    private EventStoreClientSettings? _settings;

    private string _streamName = "default";


    public string GetCurrentStoreStream() => _streamName;
    
    public void SetStoreStream(string streamName)
    {
        _streamName = streamName;
    }
    
    public async ValueTask Raise(string eventName, object content)
    {
        await using var client = await CreateClient();

        var metaData = new EventMetadata(eventName, DateTimeOffset.Now);
        
        var contentJson = JsonSerializer.Serialize(content);
        var metaDataJson = JsonSerializer.Serialize(metaData);

        var encodedData = Encoding.UTF8.GetBytes(contentJson);
        var encodedMetaData = Encoding.UTF8.GetBytes(metaDataJson);
        
        var eventData = new EventData(Uuid.NewUuid(), _streamName, encodedData, encodedMetaData);

        await client.AppendToStreamAsync(_streamName, StreamState.Any, [eventData]);
    }

    public async ValueTask<IEnumerable<Event>> GetEvents(long firstNumber, long lastNumber)
    {
        await using var client = await CreateClient();

        var count = lastNumber - firstNumber + 1;
        var resolvedEvents = client.ReadStreamAsync(Direction.Forwards, _streamName, StreamPosition.FromInt64(firstNumber), count);

        var state = await resolvedEvents.ReadState;
        if (state != ReadState.Ok)
            return [];
        
        var result = new List<Event>();
        await foreach (var resolvedEvent in resolvedEvents)
        {
            var encodedData = resolvedEvent.Event.Data;
            var encodedMetaData = resolvedEvent.Event.Metadata;

            var contentJson = Encoding.UTF8.GetString(encodedData.Span);
            var metaDataJson = Encoding.UTF8.GetString(encodedMetaData.Span);
            
            var metaData = JsonSerializer.Deserialize<EventMetadata>(metaDataJson) 
                           ?? throw new InvalidOperationException();

            var sequenceNumber = resolvedEvent.OriginalEventNumber.ToInt64();

            var @event = new Event(sequenceNumber, metaData.OccuredAt, metaData.EventName, contentJson, _streamName);
            result.Add(@event);
        }

        return result;
    }

    private async ValueTask<EventStoreClient> CreateClient()
    {
        if (_settings == null)
        {
            var connectionString = await connectionSource.GetConnectionAsync();
            _settings = EventStoreClientSettings.Create(connectionString);
        }

        return new EventStoreClient(_settings);
    }
    
    private record EventMetadata(string EventName, DateTimeOffset OccuredAt);
}