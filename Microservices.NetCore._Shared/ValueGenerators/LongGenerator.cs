namespace Microservices.NetCore.Shared.ValueGenerators;

public class LongGenerator(long startValue = 0) : IValueGenerator<long>
{
    private long _next = startValue;
    
    public long GenerateNext()
    {
        var next = Interlocked.Increment(ref _next);
        return next - 1;
    }
    
    public void Reset()
    {
        _next = startValue;
    }
}