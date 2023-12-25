namespace EasyReport.WebApi.Cache;

public interface ICacheService
{
    public Task<T?> GetAsync<T>(string key) where T : class;
    public Task SetAsync<T>(string key, T value) where T : class;
    public Task SetAsync<T>(string key, T value, TimeSpan expiration) where T : class;
    public Task RemoveAsync(string key);
    public Task<bool> ExistsAsync(string key);
}