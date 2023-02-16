using Mint.Common;
using Mint.Common.Buffer;
using Mint.Common.Config;
using Mint.Protocol.Database;
using System.Net;
using System.Net.Sockets;

namespace Mint.Protocol.Listener;

public class PacketListener
{
    private const int MAX_PACKET_SIZE = 2097151;

    // Dependencies
    private readonly IConfiguration config;
    protected readonly Logger logger;

    // Listener Data
    private readonly CancellationTokenSource cancellation;
    private CancellationTokenRegistration callback;
    private readonly IPEndPoint address;
    private readonly TcpListener listener;

    // Protocol Data
    private readonly PacketDatabase database;
    private readonly string[] versions;

    // State
    private bool running = false;
    private State state;

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

        this.state = State.STATUS;
    }

    public void Start()
    {
        listener.Start();
        this.callback = cancellation.Token.Register(listener.Stop);
    }

    public void Stop()
    {
        cancellation.Cancel();
        callback.Unregister();
    }

    public async void Listen()
    {
        logger.Info($"Listening on '{address.Address}:{address.Port}'");
        running = true;
        while (!cancellation.IsCancellationRequested)
        {
            try
            {
                var sock = await listener.AcceptSocketAsync();
                if (sock.Connected && !cancellation.IsCancellationRequested)
                {
                    byte[] bytes = new byte[MAX_PACKET_SIZE];
                    var len = await sock.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                    var buf = ByteBuf.WrapPacketBuffer(len, bytes);

                    var id = buf.ReadVarInt();
                    var packet = database.GetPacket(id, versions, Bound.CLIENT, state);
                    if (packet is not null)
                    {
                        logger.Debug($"Recieved packet '{id}/{packet.name}' in {len} bytes");
                        Handle(packet, buf);
                    } else
                    {
                        throw new NullReferenceException($"No packet with criteria [ID: '{id}' Versions: '{String.Join(", ", versions)}', Bound: 'CLIENT', State: '{state}']");
                    }
                }
            }
            catch (SocketException) when (cancellation.IsCancellationRequested)
            {
                logger.Debug("Stopping listener . . .");
            }
            catch (Exception ex)
            {
                logger.Fatal($"Listener failed: {ex}");
            }
        }
        running = false;
        logger.Info("Listener stopped");
    }

    void Handle(PacketDatabase.Protocol.Packet packet, ByteBuf buf)
    {
        List<object> values = database.ParseValues(packet, buf);
    }

    public bool IsRunning() => running;
}
