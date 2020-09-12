using Bogus;
using FluentAssertions;
using Medium.Core.Common.Builder;
using Medium.Core.Common.Extension;
using Medium.Core.Domain;
using System;
using Xunit;

namespace Medium.UnitTest.Domain
{
    public class TagTest
    {
        [Fact]
        public void ShouldBeCreated_NewAuthor()
        {
            var faker = new Faker("pt_BR");
            var expectedTag = new
            {
                Id = Guid.NewGuid(),
                Name = faker.Random.String2(8),
                AuthorId = faker.Random.Guid(),
                CreatedAt = DateTime.Now.DefaultFormat(),
                UpdatedAt = DateTime.Now.DefaultFormat()
            };

            var tag = new Tag
            {
                Id = expectedTag.Id,
                Name = expectedTag.Name,
                AuthorId = expectedTag.AuthorId
            };

            expectedTag.Should().BeEquivalentTo(tag, x => x
                .Excluding(xx => xx.Author));
        }

        [Fact]
        public void ShouldBeCreated_NewAuthor_UsingBuilder()
        {
            var faker = new Faker("pt_BR");
            var expectedTag = new
            {
                Id = Guid.NewGuid(),
                Name = faker.Random.String2(8),
                AuthorId = faker.Random.Guid(),
                CreatedAt = DateTime.Now.DefaultFormat(),
                UpdatedAt = DateTime.Now.DefaultFormat()
            };

            var tag = new TagBuilder()
                .WithId(expectedTag.Id)
                .WithName(expectedTag.Name)
                .WithAuthor(expectedTag.AuthorId)
                .Build();

            expectedTag.Should().BeEquivalentTo(tag, x => x
                .Excluding(xx => xx.Author));
        }
    }
}
