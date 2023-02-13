namespace Mint.Common.Util;

public class PathUtil
{
    public static string GetAppdata()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }

    public static string GetMinecraftDir()
    {
        return Path.Combine(GetAppdata(), ".minecraft");
    }

    public static string GetVersionsDir()
    {
        return Path.Combine(GetMinecraftDir(), "versions");
    }
}
