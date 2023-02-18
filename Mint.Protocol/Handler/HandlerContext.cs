using Mint.Protocol.Database;
using Mint.Protocol.Listener;

namespace Mint.Protocol.Handler;

public class HandlerContext
{
    private readonly PacketDatabase database;
    private readonly PacketListener listener;

    public HandlerContext(PacketDatabase database, PacketListener listener)
    {
        this.database = database;
        this.listener = listener;
    }

    public void SendPacket(PacketDatabase.Protocol.Packet packet)
    {
        
    }
}
