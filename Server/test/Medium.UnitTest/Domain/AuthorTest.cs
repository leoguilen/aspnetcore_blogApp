using Bogus;
using FluentAssertions;
using Medium.Core.Common.Builder;
using Medium.Core.Common.Extension;
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
