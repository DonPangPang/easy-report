using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace EasyReport.WebApi.Cache;

public class CacheService(IDistributedCache cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var value = await cache.GetStringAsync(key);
        return value is null ? null : JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value) where T : class
    {
        var options = new DistributedCacheEntryOptions();
        await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration) where T : class
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
    }

    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }


    public async Task<bool> ExistsAsync(string key)
    {
        return await cache.GetAsync(key) is not null;
    }
}