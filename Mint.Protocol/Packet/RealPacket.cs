using Mint.Protocol.Database;

namespace Mint.Protocol.Packet;

/// <summary>
/// This class links a template from the database to a
/// list of parameters.
/// </summary>
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
