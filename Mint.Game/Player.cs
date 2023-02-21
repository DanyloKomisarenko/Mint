using Mint.Protocol.Listener;

namespace Mint.Game;

public class Player
{
    public Player(Guid uuid, string displayname, Connection connection)
    {
        UUID = uuid;
        Displayname = displayname;
        Connection = connection;
    }

    public Guid UUID { get; }
    public string Displayname { get; }
    public Connection Connection { get; }
}
