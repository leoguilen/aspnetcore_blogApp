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
    public class TagServiceTest
    {
        private readonly IUnitOfWork _unit;
        private readonly ITagService _tagService;
        private readonly DataContext _dbContext;

        public TagServiceTest()
        {
            var provider = ServicesConfiguration.Configure();

            _dbContext = provider
                .GetRequiredService<DataContext>()
                .SeedTestData();
            _unit = provider.GetRequiredService<IUnitOfWork>();
            _tagService = new TagService(_unit);
        }

        #region Get All Tags

        [Fact]
        public async Task ShouldBeReturnedAllTags()
        {
            var tags = await _tagService.GetTagsAsync();

            tags.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.SatisfyRespectively(
                    tag1 =>
                    {
                        tag1.Id.Should().Be(Guid.Parse("5d5e9a28-7c3e-4c2a-8098-b866eab33e61"));
                        tag1.Name.Should().Be("Tag_1");
                        tag1.Author.Id.Should().Be(Guid.Parse("d4182477-0823-4908-be1d-af808e594306"));
                        tag1.Author.FirstName.Should().Be("João");
                        tag1.Author.Email.Should().Be("joao@email.com");
                    },
                    tag2 =>
                    {
                        tag2.Id.Should().Be(Guid.Parse("d94e6e00-96d0-4fc7-b621-c7746705b471"));
                        tag2.Name.Should().Be("Tag_2");
                        tag2.Author.Id.Should().Be(Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702"));
                        tag2.Author.FirstName.Should().Be("Maria");
                        tag2.Author.Email.Should().Be("maria@email.com");
                    });
        }

        #endregion

        #region Get Tag By Id

        [Fact]
        public async Task ShouldBeReturnedNullWhenTagWithDeterminedIdNotExists()
        {
            var notExistingId = Guid.NewGuid();

            var tag = await _tagService.GetTagByIdAsync(notExistingId);

            tag.Should().BeNull();
        }

        [Fact]
        public async Task ShouldBeReturnedTagByYourId()
        {
            var tagId = Guid.Parse("d94e6e00-96d0-4fc7-b621-c7746705b471");
            var expectedTag = new
            {
                Id = Guid.Parse("d94e6e00-96d0-4fc7-b621-c7746705b471"),
                Name = "Tag_2",
                AuthorId = Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702")
            };

            var tag = await _tagService.GetTagByIdAsync(tagId);

            expectedTag.Should().BeEquivalentTo(tag, options =>
                options.ExcludingMissingMembers());
            tag.Author.FirstName.Should().Be("Maria");
            tag.Author.Email.Should().Be("maria@email.com");
        }

        #endregion

        #region Create Tag

        [Fact]
        public async Task ShouldBeCreatedTag()
        {
            var newTag = new TagBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Tag_3")
                .WithAuthor(Guid.Parse("d4182477-0823-4908-be1d-af808e594306"))
                .Build();

            var created = await _tagService.CreateTagAsync(newTag);

            created.Should().BeTrue();
            _dbContext.Tags.Should().HaveCount(3);
        }

        #endregion

        #region Update Tag

        [Fact]
        public async Task ShouldBeReturnedFalseIfUpdatedTagNotExists()
        {
            var notExistingTag = new TagBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Tag_56")
                .Build();

            var updated = await _tagService
                .UpdateTagAsync(notExistingTag);

            updated.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldBeUpdatedExistingTag()
        {
            var newName = "Tag_100";
            var tag = await _dbContext.Tags.FirstAsync();
            tag.Name = newName;

            var updated = await _tagService
                .UpdateTagAsync(tag);

            updated.Should().BeTrue();

            var updatedTag = await _tagService
                .GetTagByIdAsync(tag.Id);

            updatedTag.Name.Should().Be(newName);
            updatedTag.UpdatedAt.Should().Be(DateTime.Now.DefaultFormat());
        }

        #endregion

        #region Delete Tag

        [Fact]
        public async Task ShouldBeReturnedFalseIfDeletedTagNotExists()
        {
            var notExistingTag = new TagBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Tag_999")
                .Build();

            var deleted = await _tagService
                .DeleteTagAsync(notExistingTag);

            deleted.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldBeReturnedFalseIfDeletedTagIdNotExists()
        {
            var notExistingId = Guid.NewGuid();

            var deleted = await _tagService
                .DeleteTagAsync(notExistingId);

            deleted.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldBeDeletedTag()
        {
            var lastTag = await _dbContext
                .Tags
                .LastAsync();

            var deleted = await _tagService
                .DeleteTagAsync(lastTag);

            deleted.Should().BeTrue();
            _dbContext.Tags.Should().HaveCount(1);

            var deletedTag = await _tagService
                .GetTagByIdAsync(lastTag.Id);

            deletedTag.Should().BeNull();
        }

        [Fact]
        public async Task ShouldBeDeletedAuthorById()
        {
            var firstTag = await _dbContext
                .Tags
                .FirstAsync();

            var deleted = await _tagService
                .DeleteTagAsync(firstTag.Id);

            deleted.Should().BeTrue();
            _dbContext.Tags.Should().HaveCount(1);

            var deletedTag = await _tagService
                .GetTagByIdAsync(firstTag.Id);

            deletedTag.Should().BeNull();
        }

        #endregion
    }
}
