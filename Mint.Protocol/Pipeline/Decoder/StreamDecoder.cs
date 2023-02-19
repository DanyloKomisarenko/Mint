using Mint.Common.Buffer;
using System.Net.Sockets;

namespace Mint.Protocol.Pipeline.Decoder;

public class StreamDecoder : ICurio<ByteBuf, NetworkStream>
{
    private const int MAX_PACKET_SIZE = 2097151;

    public ByteBuf Poke(NetworkStream input)
    {
        throw new NotImplementedException();
    }
}
