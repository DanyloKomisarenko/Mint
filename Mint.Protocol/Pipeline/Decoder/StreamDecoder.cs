using Mint.Common.Buffer;
using System.Net.Sockets;

namespace Mint.Protocol.Pipeline.Decoder;

public class StreamDecoder : ICurio<ByteBuf, NetworkStream>
{
    private const int MAX_PACKET_SIZE = 2097151;

    public ByteBuf Poke(NetworkStream input)
    {
        var bytes = new byte[MAX_PACKET_SIZE];
        int len = input.Read(bytes, 0, bytes.Length);
        if (len is 0) throw new InvalidOperationException("Read 0 bytes");
        var buf = new ByteBuf(len);
        for (int i = 0; i < len; i++) buf.WriteByte(bytes[i]);
        return buf;
    }
}
