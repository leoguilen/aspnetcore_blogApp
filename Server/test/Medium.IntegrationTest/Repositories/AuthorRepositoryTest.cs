using Bogus;
using FluentAssertions;
using Medium.Core.Common.Builder;
using Medium.Core.Common.Extension;
using Medium.Core.Repositories;
using Medium.Infrastructure.Data.Context;
using Medium.Infrastructure.Repositories;
using Medium.IntegrationTest.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Medium.IntegrationTest.Repositories
{
    public class AuthorRepositoryTest
    {
        private readonly DataContext _inMemoryDbContext;
        private readonly IAuthorRepository _authorRepository;

        public AuthorRepositoryTest()
        {
            var configServices = ServicesConfiguration.Configure();

            _inMemoryDbContext = configServices
                .GetRequiredService<DataContext>()
                .SeedTestData();
            _authorRepository = new AuthorRepository(_inMemoryDbContext);
        }

        [Fact]
        public void ShouldCanConnectInMemoryDatabase()
        {
            _inMemoryDbContext.Database.IsInMemory().Should().BeTrue();
            _inMemoryDbContext.Database.CanConnect().Should().BeTrue();
        }

        [Fact]
        public async Task ShouldBeReturnedListWithAllAuthors()
        {
            var authors = await _authorRepository.GetAllAsync();

            authors.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.SatisfyRespectively(
                    author1 =>
                    {
                        author1.Id.Should().Be(Guid.Parse("d4182477-0823-4908-be1d-af808e594306"));
                        author1.FirstName.Should().Be("João");
                        author1.Email.Should().Be("joao@email.com");
                    },
                    author2 =>
                    {
                        author2.Id.Should().Be(Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702"));
                        author2.FirstName.Should().Be("Maria");
                        author2.Email.Should().Be("maria@email.com");
                    });
        }

        [Fact]
        public async Task ShouldBeReturnedAuthorById()
        {
            var authorId = Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702");
            var expectedAuthor = new
            {
                Id = Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702"),
                FirstName = "Maria",
                Email = "maria@email.com"
            };

            var author = await _authorRepository.GetByIdAsync(authorId);

            expectedAuthor.Should().BeEquivalentTo(author, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task ShouldBeCreatedNewAuthor()
        {
            var faker = new Faker("pt_BR");
            var newAuthor = new AuthorBuilder()
                .WithId(Guid.NewGuid())
                .WithFirstName(faker.Person.FirstName)
                .WithLastName(faker.Person.LastName)
                .WithEmail(faker.Person.Email)
                .Build();

            await _authorRepository.CreateAuthorAsync(newAuthor);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var createdAuthor = await _authorRepository.GetByIdAsync(newAuthor.Id);

            newAuthor.Should().BeEquivalentTo(createdAuthor, options =>
                options.ExcludingMissingMembers());
            createdAuthor.CreatedAt.Should().Be(DateTime.Now.DefaultFormat());
            createdAuthor.UpdatedAt.Should().Be(DateTime.Now.DefaultFormat());
        }

        [Fact]
        public async Task ShouldBeUpdatedAuthor()
        {
            var faker = new Faker("pt_BR");
            var newAuthorEmail = faker.Person.Email;
            var newAuthorBio = faker.Lorem.Paragraphs();
            var firstAuthor = _inMemoryDbContext
                .Authors.First();
            firstAuthor.Email = newAuthorEmail;
            firstAuthor.Bio = newAuthorBio;

            await _authorRepository.UpdateAuthorAsync(firstAuthor);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var updatedAuthor = await _authorRepository
                .GetByIdAsync(firstAuthor.Id);

            updatedAuthor.Email.Should().Be(newAuthorEmail);
            updatedAuthor.Bio.Should().Be(newAuthorBio);
            updatedAuthor.UpdatedAt.Should()
                .Be(DateTime.Now.DefaultFormat());
        }

        [Fact]
        public async Task ShouldBeDeletedAuthorById()
        {
            var authorId = Guid.Parse("d4182477-0823-4908-be1d-af808e594306");

            await _authorRepository.DeleteAuthorAsync(authorId);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var deletedAuthor = await _authorRepository.GetByIdAsync(authorId);

            deletedAuthor.Should().BeNull();
            _inMemoryDbContext.Authors.Should().HaveCount(1);
        }

        [Fact]
        public async Task ShouldBeDeletedAuthor()
        {
            var author = _inMemoryDbContext.Authors.First();

            await _authorRepository.DeleteAuthorAsync(author);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var deletedAuthor = await _authorRepository.GetByIdAsync(author.Id);

            deletedAuthor.Should().BeNull();
            _inMemoryDbContext.Authors.Should().HaveCount(1);
        }
    }
}
