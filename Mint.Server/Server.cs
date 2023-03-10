using Mint.Common;
using Mint.Common.Config;
using Mint.Game;
using Mint.Protocol.Listener;

namespace Mint.Server;

public class Server
{
    private readonly IConfiguration config;
    private readonly Logger logger;
    private readonly PacketListener listener;
    private readonly GameServer gameserver;

    public Server(IConfiguration config, Logger logger, PacketListener listener, GameServer gameserver)
    {
        this.config = config;
        this.logger = logger;
        this.listener = listener;
        this.gameserver = gameserver;
    }

    public void Run()
    {
        try
        {
            logger.Info("Starting server . . .");
            listener.Start();
            while (listener.IsRunning()) { }
            logger.Info("Server Stopped");
        }
        catch (Exception e)
        {
            logger.Fatal($"{e.Message}: {e}");
        }
    }
}
