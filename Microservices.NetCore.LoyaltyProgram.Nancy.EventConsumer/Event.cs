namespace Microservices.NetCore.LoyaltyProgram.Nancy.EventConsumer;

public struct Event
{
    public long SequenceNumber { get; set; }
    public string Name { get; set; }
    public object Content { get; set; }
}