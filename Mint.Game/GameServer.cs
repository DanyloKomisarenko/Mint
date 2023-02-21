using Mint.Common;
using Mint.Common.Event;
using Mint.Game.Handler;
using Mint.Protocol.Events;
using Mint.Protocol.Listener;

namespace Mint.Game;

public class GameServer : IDisposable
{
    private readonly Logger logger;
    private readonly PacketListener listener;
    private readonly PacketHandlers handlers;
    private readonly List<Player> connectedplayers = new();

    public GameServer(Logger logger, PacketListener listener, PacketHandlers handlers)
    {
        this.logger = logger;
        this.listener = listener;
        this.handlers = handlers;
        handlers.RegisterHandlers();

        EventManager.RegisterListener(this);
    }

    [EventTarget]
    public void OnPacketRecieve(PacketRecieveEvent e)
    {
        var handlername = e.Packet.Template.handlername;
        if (handlername is not "") handlers.Handle(handlername, e.Connection, e.Packet);
    }

    public void AddPlayer(Player player) => connectedplayers.Add(player);

    public void Dispose()
    {
        EventManager.UnregisterListener(typeof(GameServer));
    }
}
