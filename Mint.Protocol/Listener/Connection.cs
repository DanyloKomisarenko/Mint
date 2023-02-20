using System.Net.Sockets;

namespace Mint.Protocol.Listener;

public class Connection
{
    public readonly TcpClient Client;
    public State State;

    public Connection(TcpClient client)
    {
        this.Client = client;
        this.State = State.HANDSHAKING;
    }
}
