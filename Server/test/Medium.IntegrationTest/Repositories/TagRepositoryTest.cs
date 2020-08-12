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

        [Fact]
        public async Task ShouldBeReturnedListWithAllTags()
        {
            var tags = await _tagRepository.GetAllAsync();

            tags.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.SatisfyRespectively(
                    tag1 =>
                    {
                        tag1.Id.Should().Be(Guid.Parse("5d5e9a28-7c3e-4c2a-8098-b866eab33e61"));
                        tag1.Name.Should().Be("Tag_1");
                    },
                    tag2 =>
                    {
                        tag2.Id.Should().Be(Guid.Parse("d94e6e00-96d0-4fc7-b621-c7746705b471"));
                        tag2.Name.Should().Be("Tag_2");
                    });
        }

        [Fact]
        public async Task ShouldBeReturnedTagById()
        {
            var tagId = Guid.Parse("5d5e9a28-7c3e-4c2a-8098-b866eab33e61");
            var expectedTag = new
            {
                Id = Guid.Parse("5d5e9a28-7c3e-4c2a-8098-b866eab33e61"),
                Name = "Tag_1"
            };

            var tag = await _tagRepository.GetByIdAsync(tagId);

            expectedTag.Should().BeEquivalentTo(tag, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task ShouldBeCreatedNewTag()
        {
            var faker = new Faker("pt_BR");
            var newTag = new TagBuilder()
                .WithId(Guid.NewGuid())
                .WithName(faker.Random.String2(8))
                .Build();

            await _tagRepository.CreateTagAsync(newTag);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var createdTag = await _tagRepository.GetByIdAsync(newTag.Id);

            newTag.Should().BeEquivalentTo(createdTag, options =>
                 options.ExcludingMissingMembers());
            createdTag.CreatedAt.Should().Be(DateTime.Now.DefaultFormat());
            createdTag.UpdatedAt.Should().Be(DateTime.Now.DefaultFormat());
        }

        [Fact]
        public async Task ShouldBeUpdatedTag()
        {
            var faker = new Faker("pt_BR");
            var newTagName = faker.Random.String2(8);
            var firstTag = _inMemoryDbContext
                .Tags.First();
            firstTag.Name = newTagName;

            await _tagRepository.UpdateTagAsync(firstTag);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var updatedTag = await _tagRepository
                .GetByIdAsync(firstTag.Id);

            updatedTag.Name.Should().Be(newTagName);
            updatedTag.UpdatedAt.Should()
                .Be(DateTime.Now.DefaultFormat());
        }

        [Fact]
        public async Task ShouldBeDeletedTagById()
        {
            var tagId = Guid.Parse("d94e6e00-96d0-4fc7-b621-c7746705b471");

            await _tagRepository.DeleteTagAsync(tagId);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var deletedTag = await _tagRepository.GetByIdAsync(tagId);

            deletedTag.Should().BeNull();
            _inMemoryDbContext.Tags.Should().HaveCount(1);
        }

        [Fact]
        public async Task ShouldBeDeletedTag()
        {
            var tag = _inMemoryDbContext.Tags.Last();

            await _tagRepository.DeleteTagAsync(tag);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var deletedTag = await _tagRepository.GetByIdAsync(tag.Id);

            deletedTag.Should().BeNull();
            _inMemoryDbContext.Tags.Should().HaveCount(1);
        }
    }
}
