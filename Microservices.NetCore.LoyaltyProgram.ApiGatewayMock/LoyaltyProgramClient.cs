using System.Net.Http.Json;
using Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.RequestSender;

namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock;

public class LoyaltyProgramClient(IRequestSender<HttpResponseMessage> requestSender)
{
    public async Task<HttpResponseMessage> GetUser(int userId)
    {
        var requestUri = $"/users/{userId}";
        var method = HttpMethod.Get;

        return await requestSender.SendRequest(method, requestUri);
    }

    public async Task<HttpResponseMessage> RegisterUser(LoyaltyProgramUser newUser)
    {
        var requestUri = "/users/";
        var method = HttpMethod.Post;
        var content = JsonContent.Create(newUser);

        return await requestSender.SendRequest(method, requestUri, content);
    }

    public async Task<HttpResponseMessage> UpdateUser(LoyaltyProgramUser user)
    {
        var requestUri = $"/users/{user.Id}";
        var method = HttpMethod.Put;
        var content = JsonContent.Create(user);

        return await requestSender.SendRequest(method, requestUri, content);
    }
}