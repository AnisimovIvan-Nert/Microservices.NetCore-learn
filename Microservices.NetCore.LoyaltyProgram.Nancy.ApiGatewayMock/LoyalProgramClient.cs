using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Polly;
using Polly.Retry;

namespace Microservices.NetCore.LoyaltyProgram.Nancy.ApiGatewayMock
{
    public class LoyaltyProgramClient(string loyalProgramMicroserviceBaseUri)
    {
        private const string JsonMediaType = "application/json";
        
        public async Task<HttpResponseMessage> GetUser(int userId)
        {
            var method = HttpMethod.Get;
            var requestUri = $"/users/{userId}";
             
            return await CreateRetryPolicy().ExecuteAsync(() => SendRequest(method, requestUri));
        }
        public async Task<HttpResponseMessage> RegisterUser(LoyaltyProgramUser newUser)
        {
            var method = HttpMethod.Post;
            var requestUri = "/users/";
            var content = new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, JsonMediaType);
            
            return await CreateRetryPolicy().ExecuteAsync(() => SendRequest(method, requestUri, content));
        }

        public async Task<HttpResponseMessage> UpdateUser(LoyaltyProgramUser user)
        {
            var method = HttpMethod.Put;
            var requestUri = $"/users/{user.Id}";
            var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, JsonMediaType);
            
            return await CreateRetryPolicy().ExecuteAsync(() => SendRequest(method, requestUri, content));
        }
        
        private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string uri, HttpContent? content = null)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Content = content;
            
            using var httpClient = CreateHttpClient();
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode == false)
                throw new Exception(response.StatusCode.ToString());
            return response;
        }

        private HttpClient CreateHttpClient()
        {
            var jsonMediaType = new MediaTypeWithQualityHeaderValue(JsonMediaType);
            var baseAddress = new Uri(loyalProgramMicroserviceBaseUri);
            
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(jsonMediaType);
            httpClient.BaseAddress = baseAddress;
            return httpClient;
        }
        
        private static AsyncRetryPolicy CreateRetryPolicy()
        {
            const int retryCount = 3;
            const int baseSleepDuration = 100;

            return Policy.Handle<Exception>()
                .WaitAndRetryAsync(retryCount, SleepDurationProvider, OnRetry);

            TimeSpan SleepDurationProvider(int attempt)
            {
                return TimeSpan.FromMilliseconds(baseSleepDuration * Math.Pow(2, attempt));
            }

            void OnRetry(Exception exception, TimeSpan _)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
