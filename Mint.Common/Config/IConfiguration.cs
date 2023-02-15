namespace Mint.Common.Config;

public interface IConfiguration
{
    string GetPacketRootFile();
    string[] GetProtocolVersions();
    string GetAddress();
    bool Debug();
}
