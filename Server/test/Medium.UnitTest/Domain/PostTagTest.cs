using Bogus;
using FluentAssertions;
using Medium.Core.Common.Extension;
using Medium.Core.Domain;
using System;
using Xunit;

namespace Medium.UnitTest.Domain
{
    public class PostTagTest
    {
        private readonly Post _postFake;
        private readonly Tag _tagFake;

        public PostTagTest()
        {
            _postFake = new Faker<Post>()
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Paragraph())
                .RuleFor(p => p.Content, f => f.Lorem.Text())
                .RuleFor(p => p.Attachments, f => f.Image.PicsumUrl())
                .RuleFor(p => p.AuthorId, f => f.Random.Guid())
                .Generate();

            _tagFake = new Faker<Tag>()
                .RuleFor(t => t.Id, f => f.Random.Guid())
                .RuleFor(t => t.Name, f => f.Lorem.Word())
                .Generate();
        }

        [Fact]
        public void ShouldBeCreated_NewPostTag()
        {
            var expectedPostTag = new
            {
                Id = Guid.NewGuid(),
                PostId = _postFake.Id,
                TagId = _tagFake.Id,
                CreatedAt = DateTime.Now.DefaultFormat(),
                UpdatedAt = DateTime.Now.DefaultFormat()
            };

            var postTag = new PostTag
            {
                Id = expectedPostTag.Id,
                PostId = expectedPostTag.PostId,
                TagId = expectedPostTag.TagId
            };

            expectedPostTag.Should().BeEquivalentTo(postTag,
                x => x.ExcludingMissingMembers());
        }
    }
}
