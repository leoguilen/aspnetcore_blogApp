using FluentAssertions;
using Medium.Core.Services;
using Medium.Infrastructure.Services;
using Medium.IntegrationTest;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Medium.UnitTest.Service
{
    public class CachingServiceTest
    {
        private readonly IMemoryCache _cache;
        private readonly ICacheService _cacheService;

        public CachingServiceTest()
        {
            var provider = ServicesConfiguration.Configure();
            _cache = provider.GetRequiredService<IMemoryCache>();

            _cache.Set("values", new List<string>
                { "valor1", "valor2", "valor3" });

            _cacheService = new CacheService(_cache);
        }

        [Fact]
        public void GetCacheResponseWithType()
        {
            const string key = "values";
            var expectedValues = new[] { "valor1", "valor2", "valor3" };

            var cacheResponse = _cacheService
                .GetCachedResponse<List<string>>(key);

            cacheResponse.Should()
                .NotBeNullOrEmpty().And
                .BeEquivalentTo(expectedValues);
        }

        [Fact]
        public void GetCacheResponseWithoutType()
        {
            const string key = "values";
            var values = new[] { "valor1", "valor2", "valor3" };
            var expectedJson = JsonSerializer.Serialize(values);

            var cacheResponse = _cacheService
                .GetCachedResponse(key);

            cacheResponse.Should()
                .NotBeNullOrEmpty().And
                .Be(expectedJson);
        }

        [Fact]
        public void RemoveCachedResponse()
        {
            const string key = "values";

            // Removendo dados cacheados
            _cacheService.RemoveCacheResponse(key);

            var cacheResponse = _cacheService
                .GetCachedResponse(key);

            cacheResponse.Should()
                .BeNullOrEmpty();
        }

        [Fact]
        public void SetCacheResponse()
        {
            const string key = "newValues";
            var newValues = new[] { "valor10", "valor20" };

            // Salvando valores no cache
            _cacheService.SetCacheResponse(key, newValues);

            var cacheResponse = _cacheService
                .GetCachedResponse<string[]>(key);

            cacheResponse.Should().HaveCount(2);
            cacheResponse[0].Should().Be("valor10");
            cacheResponse[1].Should().Be("valor20");
        }

        [Fact]
        public void SetCacheResponseWithTimeLive()
        {
            const string key = "newValues";
            var newValues = new[] { "valor10", "valor20" };

            // Salvando valores no cache
            _cacheService.SetCacheResponse(key, 
                newValues, TimeSpan.FromMinutes(5));

            var cacheResponse = _cacheService
                .GetCachedResponse<string[]>(key);

            cacheResponse.Should().HaveCount(2);
            cacheResponse[0].Should().Be("valor10");
            cacheResponse[1].Should().Be("valor20");
        }
    }
}
