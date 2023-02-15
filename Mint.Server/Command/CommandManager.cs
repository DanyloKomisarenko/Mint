using Mint.Common;
using Mint.Protocol.Listener;
using Mint.Server.Command.Commands;

namespace Mint.Server.Command;

public class CommandManager
{
    public readonly PacketListener Listener;
    public readonly Logger Logger;

    private readonly List<ICommand> commands = new();

    public CommandManager(PacketListener listener, Logger logger)
    {
        this.Listener = listener;
        this.Logger = logger;

        RegisterCommand(new HelpCommand());
        RegisterCommand(new QuitCommand());
    }

    public void Handle(string? cmd)
    {
        if (cmd is not null)
        {
            var pars = cmd.Split(' ');
            var command = GetCommand(pars[0]);
            if (command is not null)
            {
                string[] parameters = new string[pars.Length - 1];
                if (parameters.Length > 0) pars.CopyTo(parameters, 1);
                command.Handle(this, parameters);
            } else
            {
                Logger.Error($"Unknown command '{pars[0]}'");
            }
        }
    }

    public ICommand? GetCommand(string name)
    {
        foreach (var cmd in commands)
        {
            if (cmd.GetName().Equals(name) || cmd.GetAliases().Contains(name))
            {
                return cmd;
            }
        }

        return null;
    }

    public List<ICommand> GetCommands() => commands;

    void RegisterCommand(ICommand command) => this.commands.Add(command);
}
