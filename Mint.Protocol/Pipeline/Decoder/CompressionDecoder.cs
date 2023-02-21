using Mint.Common.Buffer;
using Mint.Protocol.Listener;

namespace Mint.Protocol.Pipeline.Decoder;

public class CompressionDecoder : ICurio<ByteBuf, ByteBuf>
{
    public ByteBuf Poke(Connection connection, ByteBuf input)
    {
        // Implement compression
        return input;
    }
}
