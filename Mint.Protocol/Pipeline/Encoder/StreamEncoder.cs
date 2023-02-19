using Mint.Common.Buffer;
using System.Net.Sockets;

namespace Mint.Protocol.Pipeline.Encoder;

public class StreamEncoder : ICurio<NetworkStream, ByteBuf>
{
    public NetworkStream Poke(ByteBuf input)
    {
        throw new NotImplementedException();
    }
}
