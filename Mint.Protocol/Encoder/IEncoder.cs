namespace Mint.Protocol.Encoder;

public interface IEncoder<O, I>
{
    O Encode(I input);
}
