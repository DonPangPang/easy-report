﻿using System.Text.Json;
using Newtonsoft.Json;

namespace EasyReport.Infrastructure.Extensions;

public static class JsonExtensions
{
    public static string? ToJson(this object? obj)
    {
        try
        {
            return obj is null ? default : JsonConvert.SerializeObject(obj);
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
            return string.IsNullOrWhiteSpace(json) || !(json.StartsWith("{") || json.StartsWith("[")) ? default : JsonConvert.DeserializeObject<T>(json);
        }
        catch
        {
            return default;
        }
    }
}