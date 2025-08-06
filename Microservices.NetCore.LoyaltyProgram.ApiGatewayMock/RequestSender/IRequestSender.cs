namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.RequestSender;

public interface IRequestSender<TResponse>
{
    public ValueTask<TResponse> SendRequest(HttpMethod method, string uri, HttpContent? content = null);
}