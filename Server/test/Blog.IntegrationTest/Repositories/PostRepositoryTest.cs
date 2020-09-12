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
    public class PostRepositoryTest
    {
        private readonly DataContext _inMemoryDbContext;
        private readonly IPostRepository _postRepository;

        public PostRepositoryTest()
        {
            var configServices = ServicesConfiguration.Configure();

            _inMemoryDbContext = configServices
                .GetRequiredService<DataContext>()
                .SeedTestData();
            _postRepository = new PostRepository(_inMemoryDbContext);
        }

        [Fact]
        public void ShouldCanConnectInMemoryDatabase()
        {
            _inMemoryDbContext.Database.IsInMemory().Should().BeTrue();
            _inMemoryDbContext.Database.CanConnect().Should().BeTrue();
        }

        [Fact]
        public async Task ShouldBeReturnedListWithAllPosts()
        {
            var posts = await _postRepository.GetAllAsync();

            posts.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.SatisfyRespectively(
                    post1 =>
                    {
                        post1.Id.Should().Be(Guid.Parse("b65afc54-d766-4377-8c89-22662582174e"));
                        post1.Title.Should().Be("Post 1");
                        post1.Content.Should().Be("First post content");
                        post1.Attachments.Split(",").Should().HaveCount(2);
                        post1.Author.Id.Should().Be(Guid.Parse("d4182477-0823-4908-be1d-af808e594306"));
                        post1.Author.FirstName.Should().Be("João");
                        post1.Author.Email.Should().Be("joao@email.com");
                    },
                    post2 =>
                    {
                        post2.Id.Should().Be(Guid.Parse("a06ba60c-c999-4de3-aa23-4f0c13bd71ad"));
                        post2.Title.Should().Be("Post 2");
                        post2.Content.Should().Be("Second post content");
                        post2.Attachments.Split(",").Should().HaveCount(2);
                        post2.Author.Id.Should().Be(Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702"));
                        post2.Author.FirstName.Should().Be("Maria");
                        post2.Author.Email.Should().Be("maria@email.com");
                    });
        }

        [Fact]
        public async Task ShouldBeReturnedPostById()
        {
            var postId = Guid.Parse("b65afc54-d766-4377-8c89-22662582174e");
            var expectedPost = new
            {
                Id = Guid.Parse("b65afc54-d766-4377-8c89-22662582174e"),
                Title = "Post 1",
                Content = "First post content",
                Attachments = "post1img1.jpg,post1img2.jpg",
                Author = new AuthorBuilder()
                    .WithId(Guid.Parse("d4182477-0823-4908-be1d-af808e594306"))
                    .WithFirstName("João")
                    .WithEmail("joao@email.com")
                    .Build()
            };

            var post = await _postRepository.GetByIdAsync(postId);

            expectedPost.Should().BeEquivalentTo(post, options => options
                .Excluding(p => p.Author.Password)
                .Excluding(p => p.Author.Salt)
                .ExcludingMissingMembers());
        }

        [Fact]
        public async Task ShouldBeCreatedNewPost()
        {
            var faker = new Faker("pt_BR");
            var newPost = new PostBuilder()
                .WithId(Guid.NewGuid())
                .WithTitle(faker.Lorem.Paragraph())
                .WithContent(faker.Lorem.Text())
                .WithAttachments(faker.Image.PicsumUrl())
                .WithAuthor(Guid.Parse("d4182477-0823-4908-be1d-af808e594306"))
                .Build();

            await _postRepository.CreatePostAsync(newPost);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var createdPost = await _postRepository.GetByIdAsync(newPost.Id);

            newPost.Should().BeEquivalentTo(createdPost, options => options
                .Excluding(p => p.Author)
                .ExcludingMissingMembers());
            createdPost.Author.FirstName.Should().Be("João");
            createdPost.Author.Email.Should().Be("joao@email.com");
            createdPost.CreatedAt.Should().Be(DateTime.Now.DefaultFormat());
            createdPost.UpdatedAt.Should().Be(DateTime.Now.DefaultFormat());
        }

        [Fact]
        public async Task ShouldBeUpdatedPost()
        {
            var faker = new Faker("pt_BR");
            var newPostTitle = faker.Lorem.Paragraph();
            var newPostContent = faker.Lorem.Text();
            var firstPost = _inMemoryDbContext
                .Posts.First();
            firstPost.Title = newPostTitle;
            firstPost.Content = newPostContent;

            await _postRepository.UpdatePostAsync(firstPost);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var updatedPost = await _postRepository
                .GetByIdAsync(firstPost.Id);

            updatedPost.Title.Should().Be(newPostTitle);
            updatedPost.Content.Should().Be(newPostContent);
            updatedPost.UpdatedAt.Should()
                .Be(DateTime.Now.DefaultFormat());
        }

        [Fact]
        public async Task ShouldBeDeletedPostById()
        {
            var postId = Guid.Parse("b65afc54-d766-4377-8c89-22662582174e");

            await _postRepository.DeletePostAsync(postId);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var deletedPost = await _postRepository.GetByIdAsync(postId);

            deletedPost.Should().BeNull();
            _inMemoryDbContext.Posts.Should().HaveCount(1);
        }

        [Fact]
        public async Task ShouldBeDeletedPost()
        {
            var post = _inMemoryDbContext.Posts.First();

            await _postRepository.DeletePostAsync(post);
            var cmdResult = await _inMemoryDbContext.SaveChangesAsync();

            // Verifica se alguma linha foi afetada
            cmdResult.Should().BeGreaterOrEqualTo(1);

            var deletedPost = await _postRepository.GetByIdAsync(post.Id);

            deletedPost.Should().BeNull();
            _inMemoryDbContext.Posts.Should().HaveCount(1);
        }
    }
}
