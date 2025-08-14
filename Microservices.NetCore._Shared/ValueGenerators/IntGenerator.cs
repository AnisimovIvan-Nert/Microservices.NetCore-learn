namespace Microservices.NetCore.Shared.ValueGenerators;

public class IntGenerator(int startValue = 0) : IValueGenerator<int>
{
    private int _next = startValue;
    
    public int GenerateNext()
    {
        var next = Interlocked.Increment(ref _next);
        return next - 1;
    }

    public void Reset()
    {
        _next = startValue;
    }
}