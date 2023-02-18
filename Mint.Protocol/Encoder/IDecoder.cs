namespace Mint.Protocol.Decoder;

public interface IDecoder<O, I>
{
    O Decode(I input);
}
