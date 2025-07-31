using System.Text;
using EventStore.Client;
using Newtonsoft.Json;

namespace Microservices.NetCore.Shared.EventFeed.Implementation.Store;

public class EventStore : IEventStore
{
    //TODO link event store
    private const string ConnectionString = "";

    private readonly EventStoreClientSettings _settings = EventStoreClientSettings.Create(ConnectionString);

    private string _eventType = "";
    
    public void SetEventType(string eventType)
    {
        _eventType = eventType;
    }
    
    public async ValueTask Raise(string eventName, object content)
    {
        await using var client = new EventStoreClient(_settings);

        var metaData = new EventMetadata(eventName, DateTimeOffset.Now);
        
        var contentJson = JsonConvert.SerializeObject(content);
        var metaDataJson = JsonConvert.SerializeObject(metaData);

        var encodedData = Encoding.UTF8.GetBytes(contentJson);
        var encodedMetaData = Encoding.UTF8.GetBytes(metaDataJson);
        
        var eventData = new EventData(Uuid.NewUuid(), _eventType, encodedData, encodedMetaData);

        await client.AppendToStreamAsync(_eventType, StreamState.Any, [eventData]);
    }

    public async ValueTask<IEnumerable<Event>> GetEvents(long firstNumber, long lastNumber)
    {
        await using var client = new EventStoreClient(_settings);

        var count = lastNumber - firstNumber;
        var resolvedEvents = client.ReadStreamAsync(Direction.Forwards, _eventType, StreamPosition.FromInt64(firstNumber), count);

        var result = new List<Event>();
        await foreach (var resolvedEvent in resolvedEvents)
        {
            var encodedData = resolvedEvent.Event.Data;
            var encodedMetaData = resolvedEvent.Event.Metadata;

            var contentJson = Encoding.UTF8.GetString(encodedData.Span);
            var metaDataJson = Encoding.UTF8.GetString(encodedMetaData.Span);

            var content = JsonConvert.DeserializeObject(contentJson) 
                          ?? throw new InvalidOperationException();
            var metaData = JsonConvert.DeserializeObject<EventMetadata>(metaDataJson) 
                           ?? throw new InvalidOperationException();

            var sequenceNumber = resolvedEvent.OriginalEventNumber.ToInt64();

            var @event = new Event(sequenceNumber, metaData.OccuredAt, metaData.EventName, content, _eventType);
            result.Add(@event);
        }

        return result;
    }

    private record EventMetadata(string EventName, DateTimeOffset OccuredAt);
}