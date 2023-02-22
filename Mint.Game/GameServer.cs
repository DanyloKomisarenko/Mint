using Mint.Common;
using Mint.Common.Event;
using Mint.Game.Handler;
using Mint.Protocol.Events;

namespace Mint.Game;

/// <summary>
/// This class contains all the game state information
/// of the server.
/// </summary>
public class GameServer : IDisposable
{
    private readonly Logger logger;
    private readonly EventManager eventmanager;
    private readonly PacketHandlers handlers;
    private readonly List<Player> connectedplayers = new();

    public GameServer(Logger logger, EventManager eventmanager, PacketHandlers handlers)
    {
        this.logger = logger;
        this.eventmanager = eventmanager;
        this.handlers = handlers;
        handlers.RegisterHandlers();

        eventmanager.RegisterListener(this);
    }

    [EventTarget]
    public void OnPacketSend(PacketSendEvent e)
    {
        logger.Info($"Sent packet '{e.Packet.Template.id}/{e.Packet.Template.name}' [Buffer: '{e.Buffer}', Parameters: '{String.Join(" ", e.Packet.Parameters)}']");
    }

    [EventTarget]
    public void OnPacketRecieve(PacketRecieveEvent e)
    {
        logger.Info($"Recieved packet '{e.Packet.Template.id}/{e.Packet.Template.name}' [Parameters: '{String.Join(", ", e.Packet.Parameters)}']");
        var handlername = e.Packet.Template.handlername;
        if (handlername is not "") handlers.Handle(handlername, e.Connection, e.Packet);
    }

    public void AddPlayer(Player player) => connectedplayers.Add(player);

    public void Dispose()
    {
        eventmanager.UnregisterListener(typeof(GameServer));
    }
}
