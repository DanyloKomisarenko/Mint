using Mint.Common.Buffer;
using Mint.Common.Event;
using Mint.Protocol.Packet;

namespace Mint.Protocol.Events;

public class PacketSendEvent : IEvent
{
    public PacketSendEvent(RealPacket input, ByteBuf actualbuf)
    {
        Packet = input;
        Buffer = actualbuf;
    }

    public RealPacket Packet { get; }
    public ByteBuf Buffer { get; }
}