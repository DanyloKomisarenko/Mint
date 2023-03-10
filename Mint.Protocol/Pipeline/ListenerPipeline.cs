using Mint.Protocol.Listener;

namespace Mint.Protocol.Pipeline;

public class ListenerPipeline
{
    private readonly List<dynamic> curios = new();

    public O? Poke<O, I>(Connection connection, I? input)
    {
        dynamic? curr = input;
        foreach (var encoder in curios)
        {
            curr = encoder.Poke(connection, curr);
        }
        return curr;
    }

    public ListenerPipeline Register<O, I>(ICurio<O, I> encoder)
    {
        curios.Add(encoder);
        return this;
    }
}
