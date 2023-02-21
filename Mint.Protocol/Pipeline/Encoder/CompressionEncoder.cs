using Mint.Common.Buffer;
using Mint.Protocol.Listener;

namespace Mint.Protocol.Pipeline.Encoder;

public class CompressionEncoder : ICurio<ByteBuf, ByteBuf>
{
    public ByteBuf Poke(Connection connection, ByteBuf input)
    {
        return input;
    }
}
