namespace Microservices.NetCore._Shared.EventFeed;

public readonly struct Event(
    long sequenceNumber,
    DateTimeOffset occuredAt,
    string name,
    object content)
{
    public long SequenceNumber { get; } = sequenceNumber;
    public DateTimeOffset OccuredAt { get; } = occuredAt;
    public string Name { get; } = name;
    public object Content { get; } = content;
}