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
    private readonly Logger logger;

    // Listener Data
    private readonly IPAddress address;
    private readonly int port;
    private readonly TcpListener listener;

    // Protocol Data
    private readonly string[] versions;
    private readonly PacketDatabase database;

    // State
    private bool running;
    private State state;

    public PacketListener(IConfiguration config, Logger logger) {
        this.config = config;
        this.logger = logger;

        string[] address = config.GetAddress().Split(":");
        this.address = IPAddress.Parse(address[0]);
        this.port = int.Parse(address[1]);
        this.listener = new(this.address, this.port);

        this.versions = config.GetProtocolVersions();
        this.database = new(config.GetPacketRootFile());

        this.running = true;
        this.state = State.STATUS;
    }

    public async Task<int> RunAsync()
    {
        var task = new Task<int>(() =>
        {
            listener.Start();
            while (running)
            {
                var sock = listener.AcceptSocket();

                try
                {
                    // Read buffer
                    byte[] bytes = new byte[MAX_PACKET_SIZE];
                    var len = sock.Receive(bytes);
                    var buf = ByteBuf.WrapPacketBuffer(len, bytes);

                    // Parse Packet
                    int id = buf.ReadVarInt();
                } catch (IndexOutOfRangeException e)
                {
                    throw new IndexOutOfRangeException($"Recieved packet was larger than maximum of '{MAX_PACKET_SIZE}' bytes", e);
                }
            }
            listener.Stop();
            return 0;
        });
    }
}
