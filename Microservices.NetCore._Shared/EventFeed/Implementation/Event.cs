namespace Microservices.NetCore.Shared.EventFeed.Implementation;

public readonly struct Event(
    long sequenceNumber,
    DateTimeOffset occuredAt,
    string name,
    string contentJson,
    string stream)
{
    public long SequenceNumber { get; } = sequenceNumber;
    public DateTimeOffset OccuredAt { get; } = occuredAt;
    public string Name { get; } = name;
    public string ContentJson { get; } = contentJson;
    public string Stream { get; } = stream;
}