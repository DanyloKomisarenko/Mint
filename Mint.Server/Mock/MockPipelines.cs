using Mint.Common;
using Mint.Common.Config;
using Mint.Common.Event;
using Mint.Protocol.Database;
using Mint.Protocol.Pipeline;
using Mint.Protocol.Pipeline.Decoder;
using Mint.Protocol.Pipeline.Encoder;

namespace Mint.Server.Mock;

public class MockPipelines : IPipelines
{
    private readonly IConfiguration config;
    private readonly Logger logger;
    private readonly EventManager eventmanager;
    private readonly PacketDatabase database;

    public MockPipelines(IConfiguration config, Logger logger, EventManager eventmanager, PacketDatabase database)
    {
        this.config = config;
        this.logger = logger;
        this.eventmanager = eventmanager;
        this.database = database;
    }

    public void RegisterPipelines(Pipelines pipelines)
    {
        // Pipelines
        pipelines.Encoders
            .Register(new PacketEncoder(eventmanager))
            .Register(new CompressionEncoder());

        pipelines.Decoders
            .Register(new FrameDecoder())
            .Register(new CompressionDecoder())
            .Register(new PacketDecoder(config, eventmanager, database));
    }
}