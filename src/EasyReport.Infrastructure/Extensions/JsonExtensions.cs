using System.Text.Json;

namespace EasyReport.Infrastructure.Extensions;

public static class JsonExtensions
{
    public static string? ToJson(this object? obj)
    {
        try
        {
            return obj is null ? default : JsonSerializer.Serialize(obj);
        }
        catch
        {
            return default;
        }
    }

    public static T? ToObject<T>(this string json)
    {
        try
        {
            return string.IsNullOrWhiteSpace(json) || !json.StartsWith("{") ? default : JsonSerializer.Deserialize<T>(json);
        }
        catch
        {
            return default;
        }
    }
}