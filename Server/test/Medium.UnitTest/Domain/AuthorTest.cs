using Bogus;
using FluentAssertions;
using Medium.Core.Common.Builder;
using Medium.Core.Common.Extension;
using Medium.Core.Domain;
using System;
using Xunit;

namespace Medium.UnitTest.Domain
{
    public class AuthorTest
    {
        [Fact]
        public void ShouldBeCreated_NewAuthor()
        {
            var faker = new Faker("pt_BR");
            var expectedAuthor = new
            {
                Id = faker.Random.Guid(),
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Username = faker.Person.UserName,
                Password = faker.Internet.Password(),
                Hash = faker.Random.AlphaNumeric(6),
                Email = faker.Person.Email,
                ConfirmedEmail = faker.Random.Bool(),
                Deactivated = faker.Random.Bool(),
                Deleted = faker.Random.Bool(),
                CreatedAt = DateTime.Now.DefaultFormat(),
                UpdatedAt = DateTime.Now.DefaultFormat()
            };

            var author = new Author
            {
                Id = expectedAuthor.Id,
                FirstName = expectedAuthor.FirstName,
                LastName = expectedAuthor.LastName,
                Username = expectedAuthor.Username,
                Password = expectedAuthor.Password,
                Hash = expectedAuthor.Hash,
                Email = expectedAuthor.Email,
                ConfirmedEmail = expectedAuthor.ConfirmedEmail,
                Deactivated = expectedAuthor.Deactivated,
                Deleted = expectedAuthor.Deleted,
            };

            expectedAuthor.Should().BeEquivalentTo(author);
        }

        [Fact]
        public void ShouldBeCreated_NewAuthor_UsingBuilder()
        {
            var faker = new Faker("pt_BR");
            var expectedAuthor = new
            {
                Id = faker.Random.Guid(),
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Username = faker.Person.UserName,
                Password = faker.Internet.Password(),
                Hash = faker.Random.AlphaNumeric(6),
                Email = faker.Person.Email,
                ConfirmedEmail = faker.Random.Bool(),
                Deactivated = faker.Random.Bool(),
                Deleted = faker.Random.Bool(),
                CreatedAt = DateTime.Now.DefaultFormat(),
                UpdatedAt = DateTime.Now.DefaultFormat()
            };

            var author = new AuthorBuilder()
                .WithId(expectedAuthor.Id)
                .WithFirstName(expectedAuthor.FirstName)
                .WithLastName(expectedAuthor.LastName)
                .WithUsername(expectedAuthor.Username)
                .WithPassword(expectedAuthor.Password)
                .WithHash(expectedAuthor.Hash)
                .WithEmail(expectedAuthor.Email)
                .WithConfirmedEmail(expectedAuthor.ConfirmedEmail)
                .WithDeactivated(expectedAuthor.Deactivated)
                .WithDeleted(expectedAuthor.Deleted)
                .Build();

            expectedAuthor.Should().BeEquivalentTo(author);
        }
    }
}
