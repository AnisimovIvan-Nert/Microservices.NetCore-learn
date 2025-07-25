using System.Text.Json;

namespace Microservices.NetCore.LoyaltyProgram.Shared.ApiGatewayMock;

public class Program
{
    private const string ExitCommand = "exit";
    private const string QueryCommand = "q";
    private const string RegisterCommand = "r";
    private const string UpdateCommand = "u";
        
    private static LoyaltyProgramClient _client;

    public static void Main(string[] arg)
    {
        Console.WriteLine("Enter LoyaltyProgram uri:");
        var uri = Console.ReadLine() ?? throw new InvalidOperationException();
            
        _client = new LoyaltyProgramClient(uri);
            
        Console.WriteLine("Welcome to the API Gateway Mock.");
        bool exit;
        do
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("********************");
            Console.WriteLine("Choose one of:");
            Console.WriteLine($"{QueryCommand} <userid> - to query the Loyalty Program Microservice for a user with id <userid>.");
            Console.WriteLine($"{RegisterCommand} <name> - to register a user with id <userid> with the Loyalty Program Microservice.");
            Console.WriteLine($"{UpdateCommand} <userid> <interests> - to update a user with new interests");
            Console.WriteLine($"{ExitCommand} - to exit");
            Console.WriteLine("********************");
                
            var command = Console.ReadLine() ?? throw new InvalidOperationException();
            exit = ProcessCommand(command);
        } while (exit == false);
    }

    private static bool ProcessCommand(string command)
    {
        if (command.Equals(ExitCommand))
            return true;
        if (command.StartsWith(QueryCommand))
            ProcessUserQuery(command);
        else if (command.StartsWith(RegisterCommand))
            ProcessUserRegistration(command);
        else if (command.StartsWith(UpdateCommand))
            ProcessUpdateUser(command);
        else
            Console.WriteLine("Did not understand command :(");
        return false;
    }

    private static void ProcessUserQuery(string command)
    {
        if (int.TryParse(command[1..], out var userId))
        {
            var response = _client.GetUser(userId).Result;
            PrettyPrintResponse(response);
        }
        else
            Console.WriteLine("Please specify user id as an int");
    }

    private static void ProcessUserRegistration(string cmd)
    {
        var newUser = new LoyaltyProgramUser
        {
            Id = -1,
            Name = cmd[1..].Trim(), 
            LoyaltyPoints = 0,
            Settings = new LoyaltyProgramSettings { Interests = []}
        };
        var response = _client.RegisterUser(newUser).Result;
        PrettyPrintResponse(response);
    }

    private static void PrettyPrintResponse(HttpResponseMessage response)
    {
        Console.WriteLine("Status code: " + response.StatusCode);
        Console.WriteLine("Headers: " + response.Headers.Aggregate("", (accumulate, headerPair) => accumulate + "\n\t" + headerPair.Key + ": " + headerPair.Value));
        Console.WriteLine("Body: " +  response.Content.ReadAsStringAsync().Result);
    }
        
    private static void ProcessUpdateUser(string cmd)
    {
        if (int.TryParse(cmd.Split(' ').Skip(1).First(), out var userId))
        {
            var response = _client.GetUser(userId).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK) 
                return;
                
            var user = JsonSerializer.Deserialize<LoyaltyProgramUser>(
                response.Content.ReadAsStringAsync().Result) ?? throw new InvalidOperationException();
            var newInterests = cmd[cmd.IndexOf(' ', 2)..].Split(',').Select(i => i.Trim());
            user.Settings =
                new LoyaltyProgramSettings
                {
                    Interests = user.Settings.Interests.Union(newInterests).ToArray()
                };
            PrettyPrintResponse(_client.UpdateUser(user).Result);
        }
        else
            Console.WriteLine("Please specify user id as an int");
    }
}