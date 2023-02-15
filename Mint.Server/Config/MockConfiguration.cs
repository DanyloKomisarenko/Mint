using Mint.Common.Config;

namespace Mint.Server.Config;

public class MockConfiguration : IConfiguration
{
    public string GetAddress()
    {
        return "127.0.0.1:25565";
    }

    public string GetPacketRootFile()
    {
        return "./Packets/root.json";
    }

    public string[] GetProtocolVersions()
    {
        return new string[] { "761" };
    }
}
