﻿namespace Microservices.NetCore._Shared.EventFeed;

public interface IEventStore
{
    ValueTask<IEnumerable<Event>> GetEvents(long firstEvent, long lastEvent);
    ValueTask Raise(string eventName, object content);
}