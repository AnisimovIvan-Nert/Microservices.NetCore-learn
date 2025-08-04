using System.Text.Json;
using Microservices.NetCore.Shared.EventFeed;

namespace Microservices.NetCore.Shared.Tests.EventFeed.Store;

public abstract class EventStoreTestBase
{
    private const string Name = "name";
    private static readonly object Content = new ContentRecord("content");

    protected abstract ValueTask<IEventStore> CreateEventStore();
    
    [Fact]
    public async Task SetStoreStream_BeforeRaise_ChangeEventRaisingLocation()
    {
        const string storingStream = nameof(SetStoreStream_BeforeRaise_ChangeEventRaisingLocation);
        
        var store = await CreateEventStore();
        var originalStream = store.GetCurrentStoreStream();
        store.SetStoreStream(storingStream);
        
        
        await store.Raise(Name, Content);
        
        
        var storedEvents = (await store.GetEvents(0, 10)).ToArray();
        Assert.Single(storedEvents);
        Assert.Equal(storingStream, storedEvents.Single().Stream);
        
        store.SetStoreStream(originalStream);
        var originalStoredEvents = await store.GetEvents(0, 10);
        Assert.Empty(originalStoredEvents);
    }
    
    [Fact]
    public async Task Raise_Single_AddSingleEvent()
    {
        var store = await CreateEventStore();
        store.SetStoreStream(nameof(Raise_Single_AddSingleEvent));

        await store.Raise(Name, Content);

        var storedEvents = await store.GetEvents(0, 10);
        var storedEvent = storedEvents.Single();
        Assert.Equal(Name, storedEvent.Name);
        Assert.Equal(0, storedEvent.SequenceNumber);
        var content = JsonSerializer.Deserialize<ContentRecord>(storedEvent.ContentJson);
        Assert.Equal(Content, content);
    }
    
    [Fact]
    public async Task Raise_Multiple_AddMultipleEvent()
    {
        const int multipleEventCount = 10;
        
        var store = await CreateEventStore();
        store.SetStoreStream(nameof(Raise_Multiple_AddMultipleEvent));

        for (var i = 0; i < multipleEventCount; i++)
            await store.Raise(Name, Content);

        var storedEventsEnumerable = await store.GetEvents(0, multipleEventCount * 2);
        var storedEvents = storedEventsEnumerable.ToArray();
        Assert.Equal(multipleEventCount, storedEvents.Length);
        for (var i = 0; i < multipleEventCount; i++)
        {
            var storedEvent = storedEvents[i];
            Assert.Equal(Name, storedEvent.Name);
            Assert.Equal(i, storedEvent.SequenceNumber);
            var content = JsonSerializer.Deserialize<ContentRecord>(storedEvent.ContentJson);
            Assert.Equal(Content, content);
        }
    }
    
    [Theory]
    [InlineData(1, 0, 1, 1)]
    [InlineData(5, 0, 10, 5)]
    [InlineData(10, 0, 9, 10)]
    [InlineData(10, 0, 5, 6)]
    [InlineData(10, 9, 20, 1)]
    public async Task GetEvents_WithValidRange_ReturnAllEventsInRange(int actual, int rangeStart, int rangeEnd, int expected)
    {
        var store = await CreateEventStore();
        store.SetStoreStream(nameof(Raise_Multiple_AddMultipleEvent) + Guid.NewGuid());

        for (var i = 0; i < actual; i++)
            await store.Raise(Name, Content);
        
        var storedEvents = await store.GetEvents(rangeStart, rangeEnd);
        Assert.Equal(expected, storedEvents.Count());
    }

    [Theory]
    [InlineData(1, 5, 5, 10)]
    [InlineData(2, 0, 0, 10)]
    public async Task GetEvents_WithInvalidRange_ReturnEmpty(int testId, int eventCount, int rangeStart, int rangeEnd)
    {
        var store = await CreateEventStore();
        store.SetStoreStream(nameof(Raise_Multiple_AddMultipleEvent) + Guid.NewGuid());

        for (var i = 0; i < eventCount; i++)
            await store.Raise(Name, Content);
        
        var storedEvents = await store.GetEvents(rangeStart, rangeEnd);
        Assert.Empty(storedEvents);
    }

    private record ContentRecord(string Value);
}