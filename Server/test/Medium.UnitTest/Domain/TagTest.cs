using Bogus;
using FluentAssertions;
using Medium.Core.Common.Builder;
using Medium.Core.Common.Extension;
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
                CreatedAt = DateTime.Now.DefaultFormat(),
                UpdatedAt = DateTime.Now.DefaultFormat()
            };

            var tag = new Tag
            {
                Id = expectedTag.Id,
                Name = expectedTag.Name
            };

            expectedTag.Should().BeEquivalentTo(tag);
        }

        [Fact]
        public void ShouldBeCreated_NewAuthor_UsingBuilder()
        {
            var faker = new Faker("pt_BR");
            var expectedTag = new
            {
                Id = Guid.NewGuid(),
                Name = faker.Random.String2(8),
                CreatedAt = DateTime.Now.DefaultFormat(),
                UpdatedAt = DateTime.Now.DefaultFormat()
            };

            var tag = new TagBuilder()
                .WithId(expectedTag.Id)
                .WithName(expectedTag.Name)
                .Build();

            expectedTag.Should().BeEquivalentTo(tag);
        }
    }
}
