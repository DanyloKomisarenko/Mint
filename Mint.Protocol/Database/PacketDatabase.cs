using Mint.Common;
using Mint.Common.Util;

namespace Mint.Protocol.Database;

public class PacketDatabase
{
    private readonly Logger logger = new();

    private readonly Root root;
    private readonly Dictionary<ProtocolInfo, Protocol> protocols = new();

    public PacketDatabase(string rootfile)
    {
        this.root = JsonUtil.ReadFromFile<Root>(rootfile);

        logger.StartEvent($"Searching for protocols . . .");
        var rootdir = Directory.GetParent(rootfile);
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
        logger.EndEvent($"{protocols.Count} protocol(s) were found");
    }

    public Protocol.Packet GetPacket(int id, string[] protocolversions, Bound bound, State state)
    {
        foreach (string pv in protocolversions)
        {
            GetProtocolByVersion(pv).packets.First<Protocol.Packet>();
        }

        return null;
    }

    public Protocol GetLatest()
    {
        var latest = root.latest;
        if (latest is not null)
        {
            return GetProtocolByIdentifier(latest);
        } else
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

        public Packet[]? packets;

        public record Packet
        {
            public int? id;
            public string? name;
            public string? bound;
            public string? state;
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
