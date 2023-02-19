using Mint.Common.Buffer;
using Mint.Protocol.Packet;

namespace Mint.Protocol.Pipeline.Decoder;

public class PacketDecoder : ICurio<RealPacket, ByteBuf>
{
    public RealPacket Poke(ByteBuf input)
    {
        throw new NotImplementedException();
    }
}
