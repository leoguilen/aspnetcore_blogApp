using Medium.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text.Json;

namespace Medium.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T GetCachedResponse<T>(string key)
        {
            if (!_memoryCache.TryGetValue(key, out T response))
            {
                return default;
            }

            return response;
        }

        public string GetCachedResponse(string key)
        {
            if (!_memoryCache.TryGetValue(key, out var response))
            {
                return default;
            }

            return JsonSerializer
                .Serialize(response);
        }

        public void RemoveCacheResponse(string key)
        {
            var cachedResponse = GetCachedResponse(key);

            if (!string.IsNullOrEmpty(cachedResponse))
                _memoryCache.Remove(key);
        }

        public void SetCacheResponse<T>(string key, T response)
        {
            if (string.IsNullOrEmpty(GetCachedResponse(key)))
                _memoryCache.Set(key, response);
        }

        public void SetCacheResponse<T>(string key, T response, TimeSpan timeLive)
        {
            if (string.IsNullOrEmpty(GetCachedResponse(key)))
            {
                var cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(6),
                    Priority = CacheItemPriority.Normal,
                    SlidingExpiration = timeLive
                };

                _memoryCache.Set(key, response, cacheExpirationOptions);
            }
        }
    }
}
