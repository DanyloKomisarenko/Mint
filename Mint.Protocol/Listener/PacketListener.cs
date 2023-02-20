using Mint.Common;
using Mint.Common.Buffer;
using Mint.Common.Config;
using Mint.Common.Error;
using Mint.Protocol.Database;
using Mint.Protocol.Packet;
using Mint.Protocol.Pipeline;
using System.Net;
using System.Net.Sockets;

namespace Mint.Protocol.Listener;

public class PacketListener : IDisposable
{
    private const int MAX_PACKET_SIZE = 2097151;

    // Dependencies
    private readonly IConfiguration config;
    private readonly Logger logger;

    // Listener Data
    private readonly CancellationTokenSource cancellation;
    private CancellationTokenRegistration callback;
    private readonly IPEndPoint address;
    private readonly TcpListener listener;
    private readonly Pipelines pipelines;
    private readonly Dictionary<TcpClient, Connection> connections = new();

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
        this.callback = cancellation.Token.Register(Dispose);
    }

    public void Dispose()
    {
        running = false;
        listener.Stop();
        cancellation.Cancel();
        callback.Unregister();
    }

    public void HandleClient(IAsyncResult ar)
    {
        // Connect to client
        listener.BeginAcceptTcpClient(HandleClient, listener);
        using var client = listener.EndAcceptTcpClient(ar);

        try
        {
            // Prepare socket
            using var sock = client.Client;
            if (sock is null) throw new NullReferenceException($"Failed to open socket");
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // Register connection
            connections[client] = new(client);

            // Handle client
            while (sock.Connected && !cancellation.IsCancellationRequested)
            {
                using var stream = client.GetStream();
                byte[] bytes = new byte[MAX_PACKET_SIZE];
                int len = stream.Read(bytes, 0, bytes.Length);
                var buf = new ByteBuf(len);
                for (int i = 0; i < len; i++) buf.WriteByte(bytes[i]);

                while (stream.CanRead)
                {
                    var decoded = pipelines.PokeDecoders<RealPacket, ByteBuf>(buf);
                    int result = pipelines.PokeHandlers<int, RealPacket>(decoded);
                    if (result is not 0) throw new MintException("Failed to handle packet", new InvalidOperationException(), (Status)result);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Fatal($"Listener failed: {ex}");
        }

        connections.Remove(client);
    }

    public Connection GetConnection(TcpClient client) => connections[client];
    public bool IsRunning() => running;
}
