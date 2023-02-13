namespace Mint.Common.Util;

public class HttpUtil
{
    public static async Task<byte[]> DownloadBytesAsync(string url)
    {
        byte[] bytes;
        using (var client = new HttpClient())
        {
            using var response = await client.GetAsync(url);
            bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        if (bytes is null) throw new NullReferenceException($"Failed to download bytes from '{url}'");
        return bytes;
    }
}
