using Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.HttpClientFactory;
using Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.RetryPolicyFactory;

namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.RequestSender;

public class HttpRequestSender(
    IHttpClientFactory httpClientFactory,
    IRetryPolicyFactory retryPolicyFactory)
    : IRequestSender<HttpResponseMessage>
{
    public async ValueTask<HttpResponseMessage> SendRequest(HttpMethod method, string uri, HttpContent? content = null)
    {
        var request = new HttpRequestMessage(method, uri);
        request.Content = content;

        var retryPolicy = retryPolicyFactory.CreateAsync<Exception>();
        using var httpClient = httpClientFactory.Create();
        
        return await retryPolicy.ExecuteAsync(() => httpClient.SendAsync(request));
    }
}