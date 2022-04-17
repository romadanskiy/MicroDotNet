namespace RuOverflow.Questions.Infrastructure.Cache;

public interface ICache
{
    public Task AddAsync<T>(string key, T value, TimeSpan? lifeTile = null);

    public Task AddAsync<T>(Guid key, T value, TimeSpan? lifeTile = null);

    public Task AddAsync<TKey, T>(TKey key, T value, TimeSpan? lifeTile = null);

    public Task<T?> GetAsync<T>(string key);

    public Task<T?> GetAsync<T>(Guid key);

    public Task<T?> GetAsync<TKey, T>(TKey key);

    public Task RemoveAsync(Guid key);

    public Task RemoveAsync(string key);

    public Task RemoveAsync<TKey>(TKey key);
}
