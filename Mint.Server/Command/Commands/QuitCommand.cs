namespace Mint.Server.Command.Commands;

public class QuitCommand : ICommand
{
    public string[] GetAliases() => new string[] { "q" };
    public string GetHelp() => "Shutdowns the server";
    public string GetName() => "quit";

    public void Handle(CommandManager commands, string[] parameters)
    {
        commands.Logger.Info("Quitting server");
        commands.Listener.Stop();
    }
}
