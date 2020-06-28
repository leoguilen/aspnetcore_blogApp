using FluentAssertions;
using Medium.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Medium.IntegrationTest.Repositories
{
    public class AuthorRepository
    {
        private readonly DataContext _inMemoryDbContext;

        public AuthorRepository()
        {
            var configServices = ServicesConfiguration.Configure();

            _inMemoryDbContext = configServices.GetRequiredService<DataContext>();
        }

        [Fact]
        public void ShouldCanConnectInMemoryDatabase()
        {
            _inMemoryDbContext.Database.IsInMemory().Should().BeTrue();
            _inMemoryDbContext.Database.CanConnect().Should().BeTrue();
        }
    }
}
