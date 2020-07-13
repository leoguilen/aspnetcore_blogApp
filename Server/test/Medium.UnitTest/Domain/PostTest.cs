using Bogus;
using FluentAssertions;
using Medium.Core.Common.Builder;
using Medium.Core.Common.Extension;
using Medium.Core.Domain;
using System;
using Xunit;

namespace Medium.UnitTest.Domain
{
    public class PostTest
    {
        [Fact]
        public void ShouldBeCreated_NewAuthor()
        {
            var faker = new Faker("pt_BR");
            var expectedPost = new
            {
                Id = Guid.NewGuid(),
                Title = faker.Lorem.Paragraph(),
                Content = faker.Lorem.Text(),
                Attachments = $"{faker.Internet.Url()},{faker.Image.PicsumUrl()}",
                CreatedAt = DateTime.Now.DefaultFormat(),
                UpdatedAt = DateTime.Now.DefaultFormat()
            };

            var post = new Post
            {
                Id = expectedPost.Id,
                Title = expectedPost.Title,
                Content = expectedPost.Content,
                Attachments = expectedPost.Attachments
            };

            expectedPost.Should().BeEquivalentTo(post);
        }

        [Fact]
        public void ShouldBeCreated_NewAuthor_UsingBuilder()
        {
            var faker = new Faker("pt_BR");
            var expectedPost = new
            {
                Id = Guid.NewGuid(),
                Title = faker.Lorem.Paragraph(),
                Content = faker.Lorem.Text(),
                Attachments = $"{faker.Internet.Url()},{faker.Image.PicsumUrl()}",
                CreatedAt = DateTime.Now.DefaultFormat(),
                UpdatedAt = DateTime.Now.DefaultFormat()
            };

            var post = new PostBuilder()
                .WithId(expectedPost.Id)
                .WithTitle(expectedPost.Title)
                .WithContent(expectedPost.Content)
                .WithAttachments(expectedPost.Attachments)
                .Build();

            expectedPost.Should().BeEquivalentTo(post);
        }
    }
}
