using Mint.Common.Buffer;
using Mint.Protocol.Listener;

namespace Mint.Protocol.Pipeline.Decoder;

/// <summary>
/// Using data provided from the buffer this class decompresses the 
/// compressed data and passes to the <c>PacketDecoder</c>.
/// </summary>
public class CompressionDecoder : ICurio<ByteBuf, ByteBuf>
{
    public ByteBuf Poke(Connection connection, ByteBuf input)
    {
        ByteBuf buf;
        if (connection.ShouldCompress())
        {
            var datalen = input.ReadVarInt();
            // Implement compression
            buf = input;
        } else
        {
            buf = input;
        }
        return buf;
    }
}
