using Polly;
using Polly.Retry;

namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.RetryPolicyFactory;

public delegate TimeSpan CalculateRetryDelay(TimeSpan baseDelay, int retryAttempt);

public class WaitAndRetryPolicyFactory(
    int retryCount,
    TimeSpan baseRetryDelay,
    CalculateRetryDelay calculateRetryDelay)
    : IRetryPolicyFactory
{
    public readonly static int DefaultRetryCount = 3;
    public readonly static TimeSpan DefaultRetryDelay = TimeSpan.FromMilliseconds(100);

    public static TimeSpan DefaultCalculateRetryDelay(TimeSpan baseDelay, int retryAttempt)
    {
        return baseDelay * Math.Pow(2, retryAttempt);
    }

    public WaitAndRetryPolicyFactory() : this(DefaultRetryCount, DefaultRetryDelay, DefaultCalculateRetryDelay)
    {
    }

    public RetryPolicy Create<TException>() where TException : Exception
    {
        return Policy.Handle<Exception>().WaitAndRetry(retryCount, ProvideSleepDuration);
    }

    public AsyncRetryPolicy CreateAsync<TException>() where TException : Exception
    {
        return Policy.Handle<Exception>().WaitAndRetryAsync(retryCount, ProvideSleepDuration);
    }

    private TimeSpan ProvideSleepDuration(int attempt)
    {
        return calculateRetryDelay.Invoke(baseRetryDelay, attempt);
    }
}