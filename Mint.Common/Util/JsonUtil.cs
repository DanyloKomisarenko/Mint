using System.Text.Json;

namespace Mint.Common.Util;

public class JsonUtil
{
    public static T ReadFromFile<T>(string filename)
    {
        string jsonstr;
        using (StreamReader r = new(filename))
        {
            jsonstr = r.ReadToEnd();
        }

        if (jsonstr is null) throw new NullReferenceException($"Failed to read file '{filename}'");
        return Read<T>(jsonstr);
    }

    public static T Read<T>(string jsonstr)
    {
        T? json = JsonSerializer.Deserialize<T>(jsonstr, new JsonSerializerOptions()
        {
            IncludeFields = true,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
        });

        if (json is null) throw new NullReferenceException($"Failed to parse string '{jsonstr}'");
        return json;
    }
}
