namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.ConsoleClient;

public class ConsoleCommand(
    string name, 
    string description, 
    Action<string[]>? execute = null, 
    params string[] argumentNames)
{
    private const string Template = "{0} {1} - {2}";
    private const string ArgumentTemplate = "<{0}>";
    
    public string Name => name;
    public int ArgumentCount => argumentNames.Length;

    public void Execute(string[] arguments)
    {
        execute?.Invoke(arguments);
    }

    public static string GetCommandName(string rawInput)
    {
        return rawInput.Split(' ').First();
    }
    
    public static string[] GetCommandArguments(string rawInput)
    {
        rawInput = rawInput.Trim();
        return rawInput.Split(' ')[1..];
    }
    
    public override string ToString()
    {
        var formatedArgumentNames = argumentNames
            .Select(argumentName => string.Format(ArgumentTemplate, argumentName)).ToArray<object?>();
        var joinedArgumentNames = string.Join(" ", formatedArgumentNames);
        
        var formatedDescription = string.Format(description, formatedArgumentNames);

        return string.Format(Template, name, joinedArgumentNames, formatedDescription);
    }
}