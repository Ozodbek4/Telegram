using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Telegram.Domain.Common.Caching;
using Telegram.Infrastructure.Common.Settings;
using Telegram.Persistence.Caching.Brokers;
using Newtonsoft.Json;

using System.Text;
using Force.DeepCloner;

namespace Telegram.Infrastructure.Common.Caching.Brokers;

public class RedisDistributedCacheBroker(IOptions<CacheSettings> settings, IDistributedCache distributedCache) : ICacheBroker
{
    public readonly DistributedCacheEntryOptions _entryOptions = new DistributedCacheEntryOptions()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(settings.Value.AbsoluteExpirationTimeInSeconds),
        SlidingExpiration = TimeSpan.FromSeconds(settings.Value.SlidingExpirationTimeInSeconds)
    };

    public async ValueTask<T?> GetAsync<T>(string key)
    {
        var value = await distributedCache.GetAsync(key);

        return value is not null ? JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value)) : default;
    }

    public ValueTask<bool> TryGetAsync<T>(string key, out T? value)
    {
        var foundEntity = distributedCache.GetString(key);

        if (foundEntity is not null)
        {
            value = JsonConvert.DeserializeObject<T>(foundEntity);
            
            return ValueTask.FromResult(true);
        }

        value = default;

        return ValueTask.FromResult(false);
    }

    public async ValueTask<T?> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, CacheEntryOptions? entryOptions = null)
    {
        var foundEntity = await distributedCache.GetStringAsync(key);
        if (foundEntity is not null)
            return JsonConvert.DeserializeObject<T>(foundEntity);

        var value = await valueFactory();
        await SetAsync(key, value, entryOptions);

        return value;   
    }

    public async ValueTask SetAsync<T>(string key, T value, CacheEntryOptions? entryOptions = null)
    {
        await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value) , GetCacheEntryOptions(entryOptions));
    }
    
    public async ValueTask DeleteAsync<T>(string key)
    {
        await distributedCache.RemoveAsync(key);
    }

    public DistributedCacheEntryOptions GetCacheEntryOptions(CacheEntryOptions? entryOptions)
    {
        if (entryOptions == default || !entryOptions.AbsoluteExpirationRelativeToNow.HasValue && !entryOptions.SlidingExpiration.HasValue)
            return _entryOptions;

        var currentEntryOptions = _entryOptions.DeepClone();

        currentEntryOptions.AbsoluteExpirationRelativeToNow = entryOptions.AbsoluteExpirationRelativeToNow;
        currentEntryOptions.SlidingExpiration = entryOptions.SlidingExpiration;

        return currentEntryOptions;
    }
}