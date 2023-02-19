using Mint.Protocol.Packet;

namespace Mint.Protocol.Pipeline.Handlers;

public class PacketHandler : ICurio<int, RealPacket>
{
    public int Poke(RealPacket input)
    {
        return 0;
    }
}
