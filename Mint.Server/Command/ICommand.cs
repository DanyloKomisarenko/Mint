namespace Mint.Server.Command;

public interface ICommand
{
    string GetName();
    string[] GetAliases();
    string GetHelp();
    void Handle(CommandManager commands, string[] parameters);
}
