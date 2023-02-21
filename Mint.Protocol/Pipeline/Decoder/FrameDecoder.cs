using Mint.Common.Buffer;
using Mint.Protocol.Listener;

namespace Mint.Protocol.Pipeline.Decoder;

public class FrameDecoder : ICurio<ByteBuf, ByteBuf>
{
    /// <summary>
    /// Splits the <c>ByteBuf</c> into multiple frames using the encoded packet length.
    /// </summary>
    public ByteBuf Poke(Connection connection, ByteBuf input)
    {
        int len = input.ReadVarInt();
        var buf = new ByteBuf(len);
        for (int i = 0; i < len; i++) buf.WriteByte(input.ReadByte());
        return buf;
    }
}
