namespace Microservices.NetCore.LoyaltyProgram.ApiGatewayMock.ConsoleClient;

public class ConsoleClient
{
    private const string ExitCommand = "exit";

    private readonly string _name;
    private readonly List<ConsoleCommand> _commands;

    public ConsoleClient(string name, params ConsoleCommand[] commands)
    {
        var exitCommand = new ConsoleCommand(ExitCommand, "to exit");
        
        _name = name;
        _commands = commands.Append(exitCommand).ToList();
    }
    
    public void Run()
    {
        Console.WriteLine($"Welcome to {_name}");
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("********************");
            Console.WriteLine("Choose one of:");
            
            foreach (var command in _commands)
                Console.WriteLine(command);
            
            Console.WriteLine("********************");

            var userInput = Console.ReadLine() ?? throw new InvalidOperationException();
            var exit = ProcessUserInput(userInput);
            
            if (exit)
                break;
        }
    }

    public void Display(string message)
    {
        Console.WriteLine(message);
    }

    private bool ProcessUserInput(string userInput)
    {
        var chosenCommand = FindCommand(userInput);

        if (chosenCommand == null)
            return false;

        if (chosenCommand.Name == ExitCommand)
            return true;

        var arguments = ConsoleCommand.GetCommandArguments(userInput);
        if (arguments.Length != chosenCommand.ArgumentCount)
        {
            Display($"Command {chosenCommand.Name} " +
                    $"Expect: {chosenCommand.ArgumentCount} arguments. " +
                    $"Actual: {arguments.Length} arguments");

            return false;
        }
            
        chosenCommand.Execute(arguments);
        return false;
    }

    private ConsoleCommand? FindCommand(string userInput)
    {
        try
        {
            var commandName = ConsoleCommand.GetCommandName(userInput);
            return _commands.Single(command => command.Name == commandName);
        }
        catch (Exception)
        {
            Display("Did not understand command");
            return null;
        }
    }
}