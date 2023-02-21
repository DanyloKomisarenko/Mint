using Mint.Protocol.Listener;

namespace Mint.Protocol.Pipeline;

public class Pipelines
{
    public ListenerPipeline Encoders { get; }
    public ListenerPipeline Decoders { get; }

    public Pipelines()
    {
        this.Encoders = new();
        this.Decoders = new();
    }

    public O? PokeEncoders<O, I>(Connection connection, I? input) => Encoders.Poke<O, I>(connection, input);
    public O? PokeDecoders<O, I>(Connection connection, I? input) => Decoders.Poke<O, I>(connection, input);
}
