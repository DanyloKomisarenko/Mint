using Mint.Common.Buffer;
using Mint.Common.Event;
using Mint.Protocol.Events;
using Mint.Protocol.Listener;
using Mint.Protocol.Packet;

namespace Mint.Protocol.Pipeline.Encoder;

public class PacketEncoder : ICurio<ByteBuf, RealPacket>
{
    private static readonly Dictionary<string, Action<ByteBuf, object>> VALUE_WRITER = new()
    {
        { "LONG", (buf, o) => buf.WriteLong((long)o) },
        { "INT", (buf, o) => buf.WriteInt((int)o) },
        { "BOOL", (buf, o) => buf.WriteBool((bool)o) },
        { "USHORT", (buf, o) => buf.WriteUShort((ushort)o) },
        { "VARLONG", (buf, o) => buf.WriteVarLong((long)o) },
        { "VARINT", (buf, o) => buf.WriteVarInt((int)o) },
        { "STRING", (buf, o) => buf.WriteString((string)o) }
    };

    public ByteBuf Poke(Connection connection, RealPacket input)
    {
        var maxbuf = new ByteBuf(PacketListener.MAX_PACKET_SIZE);
        maxbuf.WriteVarInt(input.Template.id);
        var pars = input.Template.parameters;
        if (pars is not null)
        {
            for (int i = 0; i < pars.Count(); i++)
            {
                var par = pars[i];
                if (par.type is not null) VALUE_WRITER[par.type](maxbuf, input.Parameters[i]);
            }
        }
        maxbuf.Flip();

        var actualbuf = new ByteBuf(maxbuf.WriterIndex());
        for (int i = 0; i < actualbuf.Capacity(); i++) actualbuf.WriteByte(maxbuf.ReadByte());

        EventManager.Call(new PacketSendEvent(input, actualbuf));

        return actualbuf;
    }
}
