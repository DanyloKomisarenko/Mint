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
    private readonly Pipelines pipelines;

    // Protocol Data
    private readonly PacketDatabase database;
    private readonly string[] versions;

    private bool running;

    public PacketListener(IConfiguration config, Logger logger, PacketDatabase database, Pipelines pipelines)
    {
        this.config = config;
        this.logger = logger;

        this.cancellation = new();
        string[] address = config.GetAddress().Split(":");
        this.address = new(IPAddress.Parse(address[0]), int.Parse(address[1]));
        this.listener = new(this.address);
        this.pipelines = pipelines;

        this.database = database;
        this.versions = config.GetProtocolVersions();
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
                var decoded = pipelines.PokeDecoders<RealPacket, NetworkStream>(stream);
                int result = pipelines.PokeHandlers<int, RealPacket>(decoded);
                if (result is not 0) throw new Exception($"Failed to handle packet [Code: {result}]");
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
