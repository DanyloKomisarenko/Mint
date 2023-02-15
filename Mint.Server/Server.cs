using Mint.Common;
using Mint.Common.Config;
using Mint.Protocol.Listener;
using Mint.Server.Command;

namespace Mint.Server;

public class Server
{
    private readonly IConfiguration config;
    private readonly Logger logger;
    private readonly PacketListener listener;
    private readonly CommandManager commands;

    private readonly CancellationTokenSource cancellation = new();

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
                if (listener.IsRunning())
                {
                    commands.Handle(cmd);
                }
            }
            logger.Info("Server Stopped");
        }
        catch (Exception e)
        {
            logger.Fatal($"{e.Message}: {e}");
        }
    }
}
