namespace Microservices.NetCore.Shared.ValueGenerators;

public class LongGenerator : IValueGenerator<long>
{
    private long _next;
    private readonly long _startValue;

    public LongGenerator(long startValue = 0)
    {
        _startValue = startValue;
        _next = startValue;
    }

    public long GenerateNext()
    {
        var next = Interlocked.Increment(ref _next);
        return next - 1;
    }
    
    public void Reset()
    {
        _next = _startValue;
    }
}