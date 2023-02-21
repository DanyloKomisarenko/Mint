using Mint.Common.Buffer;
using Mint.Protocol.Packet;
using System.Net.Sockets;

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
    public State State;

    public Connection(PacketListener parent, TcpClient client)
    {
        this.Parent = parent;
        this.Client = client;
        this.PacketQueue = new();
        this.State = State.HANDSHAKING;

        Task.Run(() =>
        {
            while (parent.IsRunning())
            {
                if (PacketQueue.Count > 0)
                {
                    var packet = PacketQueue.Dequeue();
                    var buf = Parent.Pipelines.PokeEncoders<ByteBuf, RealPacket>(this, packet);
                    if (buf is not null)
                    {
                        byte[] bytes = buf.ReadBytes(buf.Capacity());
                        client.Client.Send(bytes);
                    }
                }
            }
        });
    }

    /// <summary>
    /// Switches from one <c>State</c> to another. Changing
    /// the packets that can be sent and recieved.
    /// </summary>
    public void ChangeState(State state)
    {
        Parent.Logger.Debug($"Changed protocol state to '{state}'");
        this.State = state;
    }

    /// <summary>
    /// Adds a <c>Packet</c> to a queue to be sent.
    /// </summary>
    public void SendPacket(int id, Bound bound)
    {
        Parent.Database.GetPacket(id, Parent.Config.GetProtocolVersions(), bound, State);
    }

    /// <summary>
    /// Adds a <c>Packet</c> to a queue to be sent.
    /// </summary>
    public void SendPacket(RealPacket packet) => PacketQueue.Enqueue(packet);
}
