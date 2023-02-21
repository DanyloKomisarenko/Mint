using Mint.Common;
using Mint.Common.Buffer;
using Mint.Common.Config;
using Mint.Common.Error;
using Mint.Common.Event;
using Mint.Protocol.Database;
using Mint.Protocol.Events;
using Mint.Protocol.Listener;
using Mint.Protocol.Packet;

namespace Mint.Protocol.Pipeline.Decoder;

/// <summary>
/// Using the frame decoded from previous decoders in the pipeline
/// this class decodes the id of the packet and uses it to fetch
/// the template from the database. Using this template it parses
/// the rest of the buffer and returns an instance of a <c>RealPacket</c>.
/// </summary>
public class PacketDecoder : ICurio<RealPacket, ByteBuf>
{
    private readonly IConfiguration config;
    private readonly Logger logger;
    private readonly PacketDatabase database;
    
    public PacketDecoder(IConfiguration config, Logger logger, PacketDatabase database)
    {
        this.config = config;
        this.logger = logger;
        this.database = database;
    }

    public RealPacket Poke(Connection connection, ByteBuf input)
    {
        var id = input.ReadVarInt();
        var template = database.GetPacket(id, config.GetProtocolVersions(), Bound.SERVER, connection.State);
        if (template != null)
        {
            // Decode Packet
            logger.Info($"Recieved packet '{id}/{template.name}' [Buffer: {input}]");
            var packet = new RealPacket(template)
            {
                Parameters = database.ParseValues(template, input)
            };

            // Handle Decoding exceptions
            if (input.ReadableBytes() > 0) 
                throw new MintException($"'{input.ReadableBytes()}' extra bytes found during decoding", new InvalidOperationException(), Status.EXTRA_BYTES);

            EventManager.Call(new PacketRecieveEvent(connection, packet));

            return packet;
        } else
        {
            throw new MintException($"Failed to find packet '{id}'", new NullReferenceException(), Status.UKNOWN_PACKET);
        }
    }
}
