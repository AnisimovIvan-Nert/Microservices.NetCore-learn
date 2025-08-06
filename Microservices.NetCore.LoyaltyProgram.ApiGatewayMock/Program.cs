using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.ConsoleClient;
using Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.HttpClientFactory;
using Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.RequestSender;
using Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.RetryPolicyFactory;
using Microservices.NetCore.Shared.ConnectionSource;

namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock;

public class Program
{
    private const string QueryCommand = "q";
    private const string RegisterCommand = "r";
    private const string UpdateCommand = "u";

    private static LoyaltyProgramClient? _client;
    private static LoyaltyProgramClient Client => _client ?? throw new InvalidOperationException();

    private static ConsoleClient.ConsoleClient? _consoleClient;
    private static ConsoleClient.ConsoleClient ConsoleClient => _consoleClient ?? throw new InvalidOperationException();

    public static void Main(string[] arg)
    {
        var connectionSource = new ConsoleConnectionSource<LoyaltyProgramClient>();
        _client = CreateLoyaltyProgramClient(connectionSource);

        var queryCommand = CreateQueryCommand();
        var registerCommand = CreateRegistrationCommand();
        var updateCommand = CreateUpdateCommand();

        _consoleClient = new ConsoleClient.ConsoleClient("API gateway", queryCommand, registerCommand, updateCommand);
        
        _consoleClient.Run();
    }

    private static ConsoleCommand CreateQueryCommand()
    {
        var name = QueryCommand;
        var description = "to query the Loyalty Program Microservice for a user with id {0}.";
        var arguments = new[] { "userId" };
        var execute = ExecuteUserQuery;

        return new ConsoleCommand(name, description, execute, arguments);

        void ExecuteUserQuery(string[] args)
        {
            if (int.TryParse(args.Single(), out var userId))
            {
                var response = Client.GetUser(userId).Result;
                PrettyDisplayResponse(response);
            }
            else
            {
                ConsoleClient.Display("Please specify userId as an int");
            }
        }
    }
    
    private static ConsoleCommand CreateRegistrationCommand()
    {
        var name = RegisterCommand;
        var description = "to register a user with {0} with the Loyalty Program Microservice.";
        var arguments = new[] { "name" };
        var execute = ExecuteUserRegistration;

        return new ConsoleCommand(name, description, execute, arguments);

        void ExecuteUserRegistration(string[] args)
        {
            var nameArgument = args[0];
            
            var newUser = new LoyaltyProgramUser
            {
                Id = -1,
                Name = nameArgument,
                LoyaltyPoints = 0,
                Settings = new LoyaltyProgramSettings { Interests = [] }
            };
            var response = Client.RegisterUser(newUser).Result;
            PrettyDisplayResponse(response);
        }
    }
    
    private static ConsoleCommand CreateUpdateCommand()
    {
        var name = UpdateCommand;
        var description = "to update a user with {0} with new interests {1}";
        var arguments = new[] { "userId", "interests" };
        var execute = ExecuteUserUpdate;

        return new ConsoleCommand(name, description, execute, arguments);

        void ExecuteUserUpdate(string[] args)
        {
            var userIdString = args[0];
            var interests = args[1];
            
            if (int.TryParse(userIdString, out var userId))
            {
                var response = Client.GetUser(userId).Result;
                if (response.StatusCode != HttpStatusCode.OK)
                    return;

                var userJson = response.Content.ReadAsStringAsync().Result;
                var user = JsonSerializer.Deserialize<LoyaltyProgramUser>(userJson, JsonSerializerOptions.Web)
                           ?? throw new InvalidOperationException();
                var newInterests = interests.Split(',');
                user.Settings =
                    new LoyaltyProgramSettings
                    {
                        Interests = user.Settings.Interests.Union(newInterests).ToArray()
                    };
                PrettyDisplayResponse(Client.UpdateUser(user).Result);
            }
            else
            {
                ConsoleClient.Display("Please specify userId as an int");
            }
        }
    }
    
    private static LoyaltyProgramClient CreateLoyaltyProgramClient(
        IConnectionSource<LoyaltyProgramClient, Uri> connectionSource)
    {
        var uri = connectionSource.GetConnectionAsync().AsTask().Result;
        var mediaType = new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json);
        var httpClientFactory = new HttpClientFactoryWithPostCreation(client =>
        {
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.Accept.Add(mediaType);
        });

        var retryPolicyFactory = new WaitAndRetryPolicyFactory();

        var httpRequestSender = new HttpRequestSender(httpClientFactory, retryPolicyFactory);

        return new LoyaltyProgramClient(httpRequestSender);
    }

    private static void PrettyDisplayResponse(HttpResponseMessage response)
    {
        ConsoleClient.Display("Status code: " + response.StatusCode);
        ConsoleClient.Display("Headers: " + response.Headers.Aggregate("",
            (accumulate, headerPair) => accumulate + "\n\t" + headerPair.Key + ": " + headerPair.Value));
        ConsoleClient.Display("Body: " + response.Content.ReadAsStringAsync().Result);
    }
}