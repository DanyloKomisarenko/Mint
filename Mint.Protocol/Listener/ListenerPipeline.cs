using Mint.Protocol.Decoder;
using Mint.Protocol.Encoder;

namespace Mint.Protocol.Listener;

public class ListenerPipeline
{
    private readonly List<IEncoder<object, object>> encoders = new();
    private readonly List<IDecoder<object, object>> decoders = new();

    public void RegisterEncoder(IEncoder<object, object> encoder) => encoders.Add(encoder);
    public void RegisterDecoder(IDecoder<object, object> decoder) => decoders.Add(decoder);
}
