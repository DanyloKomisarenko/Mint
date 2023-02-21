using Mint.Protocol.Listener;

namespace Mint.Protocol.Pipeline;

public interface ICurio<O, I>
{
    O Poke(Connection connection, I input);
}
