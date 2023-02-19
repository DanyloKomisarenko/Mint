namespace Mint.Protocol.Pipeline;

public interface ICurio<O, I>
{
    O Poke(I input);
}
