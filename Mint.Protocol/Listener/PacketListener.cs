using Mint.Common;
using Mint.Common.Config;
using Mint.Protocol.Database;
using System.Net;

namespace Mint.Protocol.Listener;

public class PacketListener : ListenerContext
{
    private const int MAX_PACKET_SIZE = 2097151;

    public PacketListener(IConfiguration config, Logger logger, PacketDatabase database) : base((ctx) =>
    {
        ctx.config = config;
        ctx.logger = logger;

        ctx.cancellation = new();
        string[] address = config.GetAddress().Split(":");
        ctx.address = new(IPAddress.Parse(address[0]), int.Parse(address[1]));
        ctx.listener = new(ctx.address);
        ctx.pipeline = new();

        ctx.database = database;
        ctx.versions = config.GetProtocolVersions();

        ctx.state = State.STATUS;
    }, InitPipeline) { }

    static void InitPipeline(ListenerPipeline pipeline)
    {

    }

    public void Start()
    {
        running = true;
        logger.Info($"Listening on '{address.Address}:{address.Port}'");
        listener.Start();
        listener.BeginAcceptTcpClient(new ClientHandler(GetContext()).HandleClient, listener);
        this.callback = cancellation.Token.Register(listener.Stop);
    }

    public void Stop()
    {
        running = false;
        cancellation.Cancel();
        callback.Unregister();
    }

    public bool IsRunning() => running;
}
