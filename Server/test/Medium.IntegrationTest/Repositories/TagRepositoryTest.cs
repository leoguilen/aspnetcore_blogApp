using FluentAssertions;
using Medium.Infrastructure.Data.Context;
using Medium.IntegrationTest.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Medium.IntegrationTest.Repositories
{
    public class TagRepositoryTest
    {
        private readonly DataContext _inMemoryDbContext;
        private readonly ITagRepository _tagRepository;

        public TagRepositoryTest()
        {
            var configServices = ServicesConfiguration.Configure();

            _inMemoryDbContext = configServices
                .GetRequiredService<DataContext>()
                .SeedTestData();
            _tagRepository = new TagRepository(_inMemoryDbContext);
        }
        
        [Fact]
        public void ShouldCanConnectInMemoryDatabase()
        {
            _inMemoryDbContext.Database.IsInMemory().Should().BeTrue();
            _inMemoryDbContext.Database.CanConnect().Should().BeTrue();
        }
    }

    public class TagRepository : ITagRepository
    {
        private readonly DataContext _inMemoryDbContext;

        public TagRepository(DataContext inMemoryDbContext)
        {
            _inMemoryDbContext = inMemoryDbContext;
        }
    }

    public interface ITagRepository
    {
        
    }
}
