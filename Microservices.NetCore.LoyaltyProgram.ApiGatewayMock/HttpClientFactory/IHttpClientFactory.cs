namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.HttpClientFactory;

public interface IHttpClientFactory
{
    public HttpClient Create();
}