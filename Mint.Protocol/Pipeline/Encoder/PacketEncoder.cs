using Mint.Common.Buffer;
using Mint.Protocol.Packet;

namespace Mint.Protocol.Pipeline.Encoder;

public class PacketEncoder : ICurio<ByteBuf, RealPacket>
{
    public ByteBuf Poke(RealPacket input)
    {
        throw new NotImplementedException();
    }
}
