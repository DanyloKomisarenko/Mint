using Mint.Protocol.Listener;

namespace Mint.Game;

/// <summary>
/// This class contains all the server 
/// side player state information.
/// </summary>
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
