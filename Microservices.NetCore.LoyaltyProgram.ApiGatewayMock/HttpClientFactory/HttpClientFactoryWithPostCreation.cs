namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.HttpClientFactory;

public class HttpClientFactoryWithPostCreation(
    Action<HttpClient>? postCreation = null)
    : IHttpClientFactory
{
    public HttpClient Create()
    {
        var httpClient = new HttpClient();
        postCreation?.Invoke(httpClient);
        return httpClient;
    }
}