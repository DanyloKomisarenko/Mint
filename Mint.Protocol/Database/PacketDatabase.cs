using Mint.Common;
using Mint.Common.Buffer;
using Mint.Common.Config;
using Mint.Common.Util;

namespace Mint.Protocol.Database;

/// <summary>
/// This class reads a json root file and loads the 
/// different protocols defined within it.
/// </summary>
public class PacketDatabase
{
    private static readonly Dictionary<string, Func<ByteBuf, object>> VALUE_READER = new()
    {
        { "LONG", (buf) => buf.ReadLong() },
        { "INT", (buf) => buf.ReadInt() },
        { "BOOL", (buf) => buf.ReadBool() },
        { "USHORT", (buf) => buf.ReadUShort() },
        { "VARLONG", (buf) => buf.ReadVarLong() },
        { "VARINT", (buf) => buf.ReadVarInt() },
        { "STRING", (buf) => buf.ReadString() }
    };

    private readonly IConfiguration config;
    private readonly Logger logger;
    private readonly Root root;
    private readonly Dictionary<ProtocolInfo, Protocol> protocols = new();

    public PacketDatabase(IConfiguration config, Logger logger)
    {
        this.root = JsonUtil.ReadFromFile<Root>(config.GetPacketRootFile());

        logger.Info($"Searching for protocols . . .");
        var rootdir = Directory.GetParent(config.GetPacketRootFile());
        if (rootdir is null) throw new DirectoryNotFoundException();
        if (root.versions is null) throw new NullReferenceException($"Root json is missing 'versions' array");
        foreach (ProtocolInfo ver in root.versions)
        {
            var protocolfile = Path.Combine(rootdir.ToString(), $"{ver.protocolVersion}.json");
            if (File.Exists(protocolfile))
            {
                protocols[ver] = JsonUtil.ReadFromFile<Protocol>(protocolfile);
                logger.Debug($"'{ver.identifier}' was found and registered");
            }
            else
            {
                logger.Debug($"'{ver.identifier}/{ver.protocolVersion}' is skipped because '{protocolfile} could not be found'");
            }
        }
        logger.Info($"{protocols.Count} protocol(s) were found");
        this.config = config;
        this.logger = logger;
    }

    public List<object> ParseValues(Protocol.Packet packet, ByteBuf buf)
    {
        List<object> o = new();
        if (packet.parameters is not null)
        {
            foreach (var par in packet.parameters)
            {
                if (par.type is not null)
                {
                    o.Add(VALUE_READER[par.type].Invoke(buf));
                } else
                {
                    throw new NullReferenceException($"Paremeter '{par.name} does not have a type'");
                }
            }
        }
        return o;
    }

    public Protocol.Packet? GetPacket(int id, string[] protocolversions, Bound bound, State state)
    {
        foreach (string pv in protocolversions)
        {
            var protocol = GetProtocolByVersion(pv);
            if (protocol is not null)
            {
                var found = protocol.packets.Where<Protocol.Packet>((packet) =>
                    packet.id.Equals(id) &&
                    packet.bound is not null && Enum.Parse(typeof(Bound), packet.bound).Equals(bound) &&
                    packet.state is not null && Enum.Parse(typeof(State), packet.state).Equals(state)
                );
                if (found.Any()) return found.First();
            } else
            {
                throw new NullReferenceException($"Failed to find protocol '{pv}'");
            }
        }

        return null;
    }

    public Protocol GetLatest()
    {
        var latest = root.latest;
        if (latest is not null)
        {
            return GetProtocolByIdentifier(latest);
        }
        else
        {
            return protocols.First().Value;
        }
    }
    public Protocol GetProtocolByIdentifier(string identifier) => protocols[protocols.Keys.First((key) => String.Compare(key.identifier, identifier) == 0)];
    public Protocol GetProtocolByVersion(string protocolVersion) => protocols[protocols.Keys.First((key) => String.Compare(key.protocolVersion, protocolVersion) == 0)];
    private Protocol GetProtocol(ProtocolInfo info) => protocols[info];

    public record Root
    {
        public string? latest;
        public ProtocolInfo[]? versions;
    }

    public record ProtocolInfo
    {
        public string? identifier;
        public string? protocolVersion;
    }

    public record Protocol
    {

        public Packet[] packets = Array.Empty<Packet>();

        public record Packet
        {
            public int id = -1;
            public string? name;
            public string? bound;
            public string? state;
            public string handlername = "";
            public string desc = "Packet is not documented yet.";
            public Parameter[]? parameters;
        }

        public record Parameter
        {
            public string? name;
            public string? type;
            public string desc = "Parameter is not documented yet.";
        }
    }
}
