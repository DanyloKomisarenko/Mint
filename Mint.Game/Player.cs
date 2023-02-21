using Mint.Protocol.Listener;

namespace Mint.Game;

/// <summary>
/// This class contains all the server 
/// side player state information.
/// </summary>
public class Player
{
    public Player(bool hasuuid, Guid uuid, string displayname, Connection connection)
    {
        HasUUID = hasuuid;
        UUID = uuid;
        Displayname = displayname;
        Connection = connection;
    }

    public bool HasUUID { get; }
    public Guid UUID { get; }
    public string Displayname { get; }
    public Connection Connection { get; }
}
