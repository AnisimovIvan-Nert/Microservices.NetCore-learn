namespace Microservices.NetCore.Shared.ValueGenerators;

public class IntGenerator : IValueGenerator<int>
{
    private int _next;
    private readonly int _startValue;

    public IntGenerator(int startValue = 0)
    {
        _startValue = startValue;
        _next = startValue;
    }

    public int GenerateNext()
    {
        var next = Interlocked.Increment(ref _next);
        return next - 1;
    }

    public void Reset()
    {
        _next = _startValue;
    }
}