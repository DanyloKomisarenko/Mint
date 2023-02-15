using Mint.Common;
using Mint.Common.Config;
using Mint.Protocol.Database;
using Mint.Protocol.Listener;
using Mint.Server.Command;

namespace Mint.Server;

public class Server
{
    private readonly IConfiguration config;
    private readonly Logger logger;
    private readonly PacketListener listener;
    private readonly CommandManager commands;

    public Server(IConfiguration config, Logger logger, PacketListener listener, CommandManager commands)
    {
        this.config = config;
        this.logger = logger;
        this.listener = listener;
        this.commands = commands;
    }

    public void Run()
    {
        try
        {
            logger.Info("Starting server . . .");
            listener.Start();
            listener.Listen();
            while (listener.IsRunning())
            {
                Console.Write("> ");
                string? cmd = Console.ReadLine();
                commands.Handle(cmd);
            }
            logger.Info("Stopped server");
        } catch(Exception e)
        {
            logger.Fatal($"{e.Message}: {e}");
        }
    }
}
