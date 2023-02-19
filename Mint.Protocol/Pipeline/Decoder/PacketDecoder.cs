using Mint.Common.Buffer;
using Mint.Protocol.Packet;

namespace Mint.Protocol.Pipeline.Decoder;

public class PacketDecoder : ICurio<RealPacket, ByteBuf>
{
    public RealPacket Poke(ByteBuf input)
    {
        var len = input.ReadVarInt();
        var id = input.ReadVarInt();

        Console.WriteLine($"Len: {len}");
        Console.WriteLine($"ID: {id}");
        Console.WriteLine($"Buffer: {input}");

        return null;
    }
}
