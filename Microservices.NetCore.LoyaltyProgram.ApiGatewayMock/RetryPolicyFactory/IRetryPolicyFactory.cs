using Polly.Retry;

namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.RetryPolicyFactory;

public interface IRetryPolicyFactory
{
    public RetryPolicy Create<TException>() where TException : Exception;
    public AsyncRetryPolicy CreateAsync<TException>() where TException : Exception;
}