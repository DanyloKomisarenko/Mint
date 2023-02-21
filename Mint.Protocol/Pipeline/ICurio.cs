using Mint.Protocol.Listener;

namespace Mint.Protocol.Pipeline;

/// <summary>
/// Represents a proccess in a pipeline.
/// </summary>
public interface ICurio<O, I>
{
    O Poke(Connection connection, I input);
}
