using Mint.Common.Event;
using Mint.Protocol.Listener;
using Mint.Protocol.Packet;

namespace Mint.Protocol.Events;

public class PacketRecieveEvent : IEvent
{
    public PacketRecieveEvent(Connection connection, RealPacket packet)
    {
        Connection = connection;
        Packet = packet;
    }

    public Connection Connection { get; }
    public RealPacket Packet { get; }
}
