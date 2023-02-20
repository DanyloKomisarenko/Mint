using Mint.Common.Buffer;

namespace Mint.Protocol.Pipeline.Decoder;

public class FrameDecoder : ICurio<ByteBuf, ByteBuf>
{
    public ByteBuf Poke(ByteBuf input)
    {
        int len = input.ReadVarInt();
        var buf = new ByteBuf(len);
        for (int i = 0; i < len; i++) buf.WriteByte(input.ReadByte());
        return buf;
    }
}
