namespace Microservices.NetCore.Shared.ValueGenerators;

public interface IValueGenerator;

public interface IValueGenerator<out TValue> : IValueGenerator
{
    TValue GenerateNext();
    void Reset();
}