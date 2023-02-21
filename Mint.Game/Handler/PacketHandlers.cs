using Mint.Protocol;
using Mint.Protocol.Listener;
using Mint.Protocol.Packet;

namespace Mint.Game.Handler;

public class PacketHandlers
{
    private readonly Dictionary<string, Handler> packethandlers = new();

    public void RegisterHandlers()
    {
        // Handshaking
        RegisterHandler("Handshake", (connection, packet) => connection.ChangeState((State)(int)packet.Parameters[3]));

        // Status
        RegisterHandler("StatusRequest", (connection, packet) => connection.SendPacket(0, Bound.CLIENT));
    }

    public void Handle(string handlername, Connection connection, RealPacket packet) => 
        packethandlers[handlername].Invoker(connection, packet);
    private void RegisterHandler(string name, Action<Connection, RealPacket> handler) => packethandlers[name] = new(handler);

    class Handler
    {
        public Handler(Action<Connection, RealPacket> invoker)
        {
            Invoker = invoker;
        }

        public Action<Connection, RealPacket> Invoker { get; }
    }
}
