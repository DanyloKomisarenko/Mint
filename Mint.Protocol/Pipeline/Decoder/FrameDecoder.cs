using Mint.Common.Buffer;
using Mint.Common.Error;
using Mint.Protocol.Listener;

namespace Mint.Protocol.Pipeline.Decoder;

public class FrameDecoder : ICurio<ByteBuf, ByteBuf>
{
    /// <summary>
    /// Splits the <c>ByteBuf</c> into multiple frames using the encoded packet length.
    /// </summary>
    public ByteBuf Poke(Connection connection, ByteBuf input)
    {
        try
        {
            int len = input.ReadVarInt();
            var buf = new ByteBuf(len);
            for (int i = 0; i < len; i++) buf.WriteByte(input.ReadByte());
            return buf;
        }
        catch (IndexOutOfRangeException e)
        {
            throw new MintException("Failed to decode frame", e, Status.MISFORMATTED_FRAME);
        }
    }
}
