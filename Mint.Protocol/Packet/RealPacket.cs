using Mint.Protocol.Database;

namespace Mint.Protocol.Packet;

public class RealPacket
{
    public RealPacket(PacketDatabase.Protocol.Packet packet)
    {
        Template = packet;
        Parameters = new();
    }

    public PacketDatabase.Protocol.Packet Template { get; }
    public List<object> Parameters { get; set; }
}
