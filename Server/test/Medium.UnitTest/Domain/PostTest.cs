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
        private readonly Author _authorFake;

        public PostTest()
        {
            _authorFake = new Faker<Author>()
                .RuleFor(u => u.Id, f => f.Random.Guid())
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Username, f => f.Person.UserName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .Generate();
        }

        [Fact]
        public void ShouldBeCreated_NewPost()
        {
            var faker = new Faker("pt_BR");
            var expectedPost = new
            {
                Id = Guid.NewGuid(),
                Title = faker.Lorem.Paragraph(),
                Content = faker.Lorem.Text(),
                Attachments = $"{faker.Internet.Url()},{faker.Image.PicsumUrl()}",
                AuthorId = _authorFake.Id,
                CreatedAt = DateTime.Now.DefaultFormat(),
                UpdatedAt = DateTime.Now.DefaultFormat()
            };

            var post = new Post
            {
                Id = expectedPost.Id,
                Title = expectedPost.Title,
                Content = expectedPost.Content,
                Attachments = expectedPost.Attachments,
                AuthorId = expectedPost.AuthorId
            };

            expectedPost.Should().BeEquivalentTo(post,
                x => x.ExcludingMissingMembers());
        }

        [Fact]
        public void ShouldBeCreated_NewPost_UsingBuilder()
        {
            var faker = new Faker("pt_BR");
            var expectedPost = new
            {
                Id = Guid.NewGuid(),
                Title = faker.Lorem.Paragraph(),
                Content = faker.Lorem.Text(),
                Attachments = $"{faker.Internet.Url()},{faker.Image.PicsumUrl()}",
                AuthorId = _authorFake.Id,
                CreatedAt = DateTime.Now.DefaultFormat(),
                UpdatedAt = DateTime.Now.DefaultFormat()
            };

            var post = new PostBuilder()
                .WithId(expectedPost.Id)
                .WithTitle(expectedPost.Title)
                .WithContent(expectedPost.Content)
                .WithAttachments(expectedPost.Attachments)
                .WithAuthor(expectedPost.AuthorId)
                .Build();

            expectedPost.Should().BeEquivalentTo(post,
                x => x.ExcludingMissingMembers());
        }
    }
}
