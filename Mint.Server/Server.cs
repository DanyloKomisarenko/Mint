using Mint.Common;
using Mint.Common.Config;
using Mint.Protocol.Listener;

namespace Mint.Server;

public class Server
{
    private readonly IConfiguration config;
    private readonly Logger logger;
    private readonly PacketListener listener;

    public Server(IConfiguration config, Logger logger, PacketListener listener)
    {
        this.config = config;
        this.logger = logger;
        this.listener = listener;
    }

    public void Run()
    {
        try
        {
            logger.Info("Starting server . . .");
            listener.Start();
            listener.Listen();
            while (listener.IsRunning()) { }
            logger.Info("Server Stopped");
        }
        catch (Exception e)
        {
            logger.Fatal($"{e.Message}: {e}");
        }
    }
}
