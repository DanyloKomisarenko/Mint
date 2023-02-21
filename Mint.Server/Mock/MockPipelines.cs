using Mint.Common;
using Mint.Common.Config;
using Mint.Protocol.Database;
using Mint.Protocol.Pipeline;
using Mint.Protocol.Pipeline.Decoder;
using Mint.Protocol.Pipeline.Encoder;

namespace Mint.Server.Mock;

public class MockPipelines : IPipelines
{
    private readonly IConfiguration config;
    private readonly Logger logger;
    private readonly PacketDatabase database;

    public MockPipelines(IConfiguration config, Logger logger, PacketDatabase database)
    {
        this.config = config;
        this.logger = logger;
        this.database = database;
    }

    public void RegisterPipelines(Pipelines pipelines)
    {
        // Pipelines
        pipelines.Encoders
            .Register(new PacketEncoder())
            .Register(new CompressionEncoder());

        pipelines.Decoders
            .Register(new FrameDecoder())
            .Register(new CompressionDecoder())
            .Register(new PacketDecoder(config, logger, database));
    }
}