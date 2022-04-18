#nullable enable
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace RuOverflow.Questions.Infrastructure.Cache;

public class Cache : ICache
{
    private readonly IDistributedCache _cache;

    public Cache(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task AddAsync<T>(string key, T value, TimeSpan? lifeTime)
    {
        var valueString = JsonConvert.SerializeObject(value);
        await _cache.SetStringAsync(key, valueString,
            new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = lifeTime });
    }

    public Task AddAsync<T>(Guid key, T value, TimeSpan? lifeTime) =>
        AddAsync(key.ToString(), value, lifeTime);


    public async Task AddAsync<TKey, T>(TKey key, T value, TimeSpan? lifeTime)
    {
        var keyString = JsonConvert.SerializeObject(key);
        await AddAsync(keyString, value, lifeTime);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var jsonString = await _cache.GetStringAsync(key);
        return jsonString is null ? default : JsonConvert.DeserializeObject<T>(jsonString);
    }

    public Task<T?> GetAsync<T>(Guid key) =>
        GetAsync<T>(key.ToString());


    public async Task<T?> GetAsync<TKey, T>(TKey key)
    {
        var keyString = JsonConvert.SerializeObject(key);
        return await GetAsync<T>(keyString);
    }

    public Task RemoveAsync(Guid key) => RemoveAsync(key.ToString());
    public Task RemoveAsync(string key) => _cache.RemoveAsync(key);

    public async Task RemoveAsync<TKey>(TKey key)
    {
        var keyString = JsonConvert.SerializeObject(key);
        await RemoveAsync(keyString);
    }
}
