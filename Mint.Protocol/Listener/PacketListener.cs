using Mint.Common;
using Mint.Common.Config;
using Mint.Protocol.Database;
using Mint.Protocol.Packet;
using Mint.Protocol.Pipeline;
using Mint.Protocol.Pipeline.Decoder;
using Mint.Protocol.Pipeline.Encoder;
using Mint.Protocol.Pipeline.Handlers;
using System.Net;
using System.Net.Sockets;

namespace Mint.Protocol.Listener;

public class PacketListener
{
    // Dependencies
    private readonly IConfiguration config;
    private readonly Logger logger;

    // Listener Data
    private  CancellationTokenSource cancellation;
    private CancellationTokenRegistration callback;
    private readonly IPEndPoint address;
    private readonly TcpListener listener;

    private readonly ListenerPipeline encoders;
    private readonly ListenerPipeline decoders;
    private readonly ListenerPipeline handlers;

    // Protocol Data
    private readonly PacketDatabase database;
    private readonly string[] versions;

    private bool running;

    public PacketListener(IConfiguration config, Logger logger, PacketDatabase database)
    {
        this.config = config;
        this.logger = logger;

        this.cancellation = new();
        string[] address = config.GetAddress().Split(":");
        this.address = new(IPAddress.Parse(address[0]), int.Parse(address[1]));
        this.listener = new(this.address);

        this.database = database;
        this.versions = config.GetProtocolVersions();

        // Pipelines
        this.encoders = new ListenerPipeline()
            .Register(new PacketEncoder())
            .Register(new CompressionEncoder())
            .Register(new StreamEncoder());

        this.decoders = new ListenerPipeline()
            .Register(new StreamDecoder())
            .Register(new CompressionDecoder())
            .Register(new PacketDecoder());

        this.handlers = new ListenerPipeline()
            .Register(new PacketHandler());
    }

    public void Start()
    {
        running = true;
        logger.Info($"Listening on '{address.Address}:{address.Port}'");
        listener.Start();
        listener.BeginAcceptTcpClient(HandleClient, listener);
        this.callback = cancellation.Token.Register(listener.Stop);
    }

    public void Stop()
    {
        running = false;
        cancellation.Cancel();
        callback.Unregister();
    }

    public void HandleClient(IAsyncResult ar)
    {
        try
        {
            logger.Debug("Opening connection . . .");
            listener.BeginAcceptTcpClient(HandleClient, listener);
            using var client = listener.EndAcceptTcpClient(ar);

            var sock = client.Client;
            if (sock is null) throw new NullReferenceException($"Failed to open socket");
            if (sock.RemoteEndPoint is not IPEndPoint connectaddress) throw new NullReferenceException($"Failed to fetch ip");
            logger.Info($"Succefully connected [IP: {connectaddress.Address}:{connectaddress.Port}]");

            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            logger.Debug("Enabled Keep-Alive");

            while (sock.Connected && !cancellation.IsCancellationRequested)
            {
                using var stream = client.GetStream();
                int result = handlers.Poke<int, RealPacket>(decoders.Poke<RealPacket, NetworkStream>(stream));
            }
        }
        catch (Exception ex)
        {
            logger.Fatal($"Listener failed: {ex}");
        }

        logger.Debug("Connection closed");
    }

    public bool IsRunning() => running;
}
