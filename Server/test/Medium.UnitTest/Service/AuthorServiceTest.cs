using FluentAssertions;
using Medium.Core.Common.Builder;
using Medium.Core.Common.Extension;
using Medium.Core.Services;
using Medium.Core.UnitOfWork;
using Medium.Infrastructure.Data.Context;
using Medium.Infrastructure.Services;
using Medium.IntegrationTest;
using Medium.IntegrationTest.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Medium.UnitTest.Service
{
    public class AuthorServiceTest
    {
        private readonly IUnitOfWork _unit;
        private readonly IAuthorService _authorService;
        private readonly DataContext _dbContext;

        public AuthorServiceTest()
        {
            var configServices = ServicesConfiguration.Configure();

            _dbContext = configServices
                .GetRequiredService<DataContext>()
                .SeedTestData();
            _unit = configServices.GetService<IUnitOfWork>();
            _authorService = new AuthorService(_unit);
        }

        #region Get All Authors

        [Fact]
        public async Task ShouldBeReturnedAllAuthors()
        {
            var authors = await _authorService.GetAuthorsAsync();

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

        #endregion

        #region Get Author By Id

        [Fact]
        public async Task ShouldBeReturnedNullWhenAuthorWithDeterminedIdNotExists()
        {
            var notExistingId = Guid.NewGuid();

            var author = await _authorService.GetAuthorByIdAsync(notExistingId);

            author.Should().BeNull();
        }

        [Fact]
        public async Task ShouldBeReturnedAuthorByYourId()
        {
            var authorId = Guid.Parse("d4182477-0823-4908-be1d-af808e594306");
            var expectedAuthor = new
            {
                Id = Guid.Parse("d4182477-0823-4908-be1d-af808e594306"),
                FirstName = "João",
                Email = "joao@email.com"
            };

            var author = await _authorService.GetAuthorByIdAsync(authorId);

            expectedAuthor.Should().BeEquivalentTo(author, options =>
                options.ExcludingMissingMembers());
        }

        #endregion

        #region Create Author

        [Fact]
        public async Task ShouldBeNotCreatedAuthorWithEqualsEmailOrUsername()
        {
            var existingAuthor = new AuthorBuilder()
                .WithFirstName("Maria")
                .WithUsername("mmaria")
                .WithEmail("maria@email.com")
                .Build();

            var created = await _authorService.CreateAuthorAsync(existingAuthor);

            created.Should().BeFalse();
            _dbContext.Authors.Should().HaveCount(2);
        }

        [Fact]
        public async Task ShouldBeCreatedAuthor()
        {
            var newAuthor = new AuthorBuilder()
                .WithFirstName("Lincoln")
                .WithUsername("lincoln2020")
                .WithEmail("lincoln@email.com")
                .Build();

            var created = await _authorService.CreateAuthorAsync(newAuthor);

            created.Should().BeTrue();
            _dbContext.Authors.Should().HaveCount(3);
        }

        #endregion

        #region Update Author

        [Fact]
        public async Task ShouldBeReturnedFalseIfUpdatedAuthorNotExists()
        {
            var notExistingAuthor = new AuthorBuilder()
                .WithFirstName("Marina")
                .WithLastName("Barbosa")
                .WithEmail("marina.barbosa@email.com")
                .Build();

            var updated = await _authorService
                .UpdateAuthorAsync(notExistingAuthor);

            updated.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldBeUpdatedExistingAuthor()
        {
            var newEmail = "novo.email@email.com";
            var newUsername = "novo.user";
            var author = await _dbContext.Authors.FirstAsync();
            author.Email = newEmail;
            author.Username = newUsername;

            var updated = await _authorService
                .UpdateAuthorAsync(author);

            updated.Should().BeTrue();

            var updatedAuthor = await _authorService
                .GetAuthorByIdAsync(author.Id);

            updatedAuthor.Email.Should().Be(newEmail);
            updatedAuthor.Username.Should().Be(newUsername);
            updatedAuthor.UpdatedAt.Should().Be(DateTime.Now.DefaultFormat());
        }

        #endregion

        #region Delete Author

        [Fact]
        public async Task ShouldBeReturnedFalseIfDeletedAuthorNotExists()
        {
            var notExistingAuthor = new AuthorBuilder()
                .WithFirstName("Juliana")
                .WithEmail("juliana@email.com")
                .WithUsername("jujuliana")
                .Build();

            var deleted = await _authorService
                .DeleteAuthorAsync(notExistingAuthor);

            deleted.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldBeReturnedFalseIfDeletedAuthorIdNotExists()
        {
            var notExistingId = Guid.NewGuid();

            var deleted = await _authorService
                .DeleteAuthorAsync(notExistingId);

            deleted.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldBeDeletedAuthor()
        {
            var lastAuthor = await _dbContext
                .Authors
                .LastAsync();

            var deleted = await _authorService
                .DeleteAuthorAsync(lastAuthor);

            deleted.Should().BeTrue();
            _dbContext.Authors.Should().HaveCount(1);

            var deletedAuthor = await _authorService
                .GetAuthorByIdAsync(lastAuthor.Id);

            deletedAuthor.Should().BeNull();
        }

        [Fact]
        public async Task ShouldBeDeletedAuthorById()
        {
            var firstAuthor = await _dbContext
                .Authors
                .FirstAsync();

            var deleted = await _authorService
                .DeleteAuthorAsync(firstAuthor.Id);

            deleted.Should().BeTrue();
            _dbContext.Authors.Should().HaveCount(1);

            var deletedAuthor = await _authorService
                .GetAuthorByIdAsync(firstAuthor.Id);

            deletedAuthor.Should().BeNull();
        }

        #endregion
    }
}
