using Mint.Common.Buffer;
using Mint.Protocol.Packet;
using System.Net.Sockets;
using static Mint.Protocol.Database.PacketDatabase.Protocol;

namespace Mint.Protocol.Listener;

/// <summary>
/// The <c>Connection</c> class represents the link between the server 
/// and client. It is used for write operations and to hold
/// the value of the state.
/// </summary>
public class Connection
{
    public readonly PacketListener Parent;
    public readonly TcpClient Client;
    public readonly Queue<RealPacket> PacketQueue;
    private State state;
    private bool compress;

    public Connection(PacketListener parent, TcpClient client)
    {
        this.Parent = parent;
        this.Client = client;
        this.PacketQueue = new();

        this.state = State.HANDSHAKING;
        this.compress = false;

        Task.Run(() =>
        {
            while (parent.IsRunning())
            {
                while (PacketQueue.Count != 0)
                {
                    var packet = PacketQueue.Dequeue();
                    var buf = Parent.Pipelines.PokeEncoders<ByteBuf, RealPacket>(this, packet);
                    if (buf is not null)
                    {
                        byte[] bytes = buf.ReadAll();
                        client.Client.Send(bytes);
                    }
                }
            }
        });
    }

    /// <summary>
    /// Widens and restricts the sendables and
    /// recievable packets.
    /// </summary>
    public State GetState() => state;
    public void ChangeState(State state)
    {
        Parent.Logger.Debug($"Changed protocol state to '{state}'");
        this.state = state;
    }

    /// <summary>
    /// Whether packets should be compressed and decompressed
    /// during the send and recieve.
    /// </summary>
    public bool ShouldCompress() => compress;
    public void SetCompress(bool compress) => this.compress = compress;

    /// <summary>
    /// Adds a <c>Packet</c> to a queue to be sent.
    /// </summary>
    public void SendPacket(int id, Bound bound, IEnumerable<object> parameters)
    {
        var template = Parent.Database.GetPacket(id, Parent.Config.GetProtocolVersions(), bound, state);
        if (template is not null)
        {
            var packet = new RealPacket(template)
            {
                Parameters = parameters.ToList()
            };
            PacketQueue.Enqueue(packet);
        }
    }
    public void SendPacket(RealPacket packet) => PacketQueue.Enqueue(packet);
}
