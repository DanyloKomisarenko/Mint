using Mint.Protocol.Pipeline.Decoder;
using Mint.Protocol.Pipeline.Encoder;
using Mint.Protocol.Pipeline.Handlers;

namespace Mint.Protocol.Pipeline;

public class Pipelines
{
    private readonly ListenerPipeline encoders;
    private readonly ListenerPipeline decoders;
    private readonly ListenerPipeline handlers;

    public Pipelines()
    {
        // Pipelines
        this.encoders = new ListenerPipeline()
            .Register(new PacketEncoder())
            .Register(new CompressionEncoder())
            .Register(new StreamEncoder());

        this.decoders = new ListenerPipeline()
            .Register(new StreamDecoder())
            .Register(new CompressionDecoder())
            .Register(new PacketDecoder());

        this.handlers = new ListenerPipeline()
            .Register(new PacketHandler());
    }

    public O? PokeEncoders<O, I>(I? input) => encoders.Poke<O, I>(input);
    public O? PokeDecoders<O, I>(I? input) => decoders.Poke<O, I>(input);
    public O? PokeHandlers<O, I>(I? input) => handlers.Poke<O, I>(input);
}
