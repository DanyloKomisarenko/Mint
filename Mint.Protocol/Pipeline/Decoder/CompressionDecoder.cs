﻿using Mint.Common.Buffer;

namespace Mint.Protocol.Pipeline.Decoder;

public class CompressionDecoder : ICurio<ByteBuf, ByteBuf>
{
    public ByteBuf Poke(ByteBuf input)
    {
        throw new NotImplementedException();
    }
}