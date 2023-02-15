namespace Mint.Server.Command.Commands;

using Mint.Server.Command;

public class HelpCommand : ICommand
{
    public string[] GetAliases() => new string[] { "h" };
    public string GetHelp() => "Returns the help information for a command";
    public string GetName() => "help";

    public void Handle(CommandManager commands, string[] parameters)
    {
        if (parameters.Length > 0)
        {
            var command = commands.GetCommand(parameters[0]);
            if (command is not null)
            {
                LogHelp(command);
            } else
            {
                throw new NullReferenceException($"Unknown command '{parameters[0]}'");
            }
        } else
        {
            foreach (var cmd in commands.GetCommands())
            {
                LogHelp(cmd);
            }
        }
    }

    void LogHelp(ICommand command)
    {
        Console.WriteLine($"{command.GetName()} - {command.GetHelp()}");
    }
}
