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
    public class PostServiceTest
    {
        private readonly IUnitOfWork _unit;
        private readonly IPostService _postService;
        private readonly DataContext _dbContext;

        public PostServiceTest()
        {
            var provider = ServicesConfiguration.Configure();

            _dbContext = provider
                .GetRequiredService<DataContext>()
                .SeedTestData();
            _unit = provider.GetService<IUnitOfWork>();
            _postService = new PostService(_unit);
        }

        #region Get All Posts

        [Fact]
        public async Task ShouldBeReturnedAllPosts()
        {
            var posts = await _postService.GetPostsAsync();

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

        #endregion

        #region Get Post By Id

        [Fact]
        public async Task ShouldBeReturnedNullWhenPostWithDeterminedIdNotExists()
        {
            var notExistingId = Guid.NewGuid();

            var post = await _postService.GetPostByIdAsync(notExistingId);

            post.Should().BeNull();
        }

        [Fact]
        public async Task ShouldBeReturnedPostByYourId()
        {
            var postId = Guid.Parse("a06ba60c-c999-4de3-aa23-4f0c13bd71ad");
            var expectedPost = new
            {
                Id = Guid.Parse("a06ba60c-c999-4de3-aa23-4f0c13bd71ad"),
                Title = "Post 2",
                Content = "Second post content",
                Attachments = "post2img1.jpg,post2img2.jpg",
                Author = new AuthorBuilder()
                    .WithId(Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702"))
                    .WithFirstName("Maria")
                    .WithEmail("maria@email.com")
                    .Build()
            };

            var post = await _postService.GetPostByIdAsync(postId);

            expectedPost.Should().BeEquivalentTo(post, options => options
                .Excluding(p => p.Author.Password)
                .Excluding(p => p.Author.Salt)
                .ExcludingMissingMembers());
        }

        #endregion

        #region Create Post

        [Fact]
        public async Task ShouldBeCreatedPost()
        {
            var newPost = new PostBuilder()
                .WithTitle("Post 3")
                .WithContent("New post created by xunit test")
                .WithAttachments("log.txt,image.jpg")
                .WithAuthor(Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702"))
                .Build();

            var created = await _postService.CreatePostAsync(newPost);

            created.Should().BeTrue();
            _dbContext.Posts.Should().HaveCount(3);
        }

        #endregion

        #region Update Post

        [Fact]
        public async Task ShouldBeReturnedFalseIfUpdatedPostNotExists()
        {
            var notExistingPost = new PostBuilder()
                .WithTitle("Post not found")
                .WithContent("It's a post not exist")
                .WithAttachments("err.txt,image.jpg")
                .Build();

            var updated = await _postService
                .UpdatePostAsync(notExistingPost);

            updated.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldBeUpdatedExistingPost()
        {
            var newTitle = "New Post";
            var newContent = "New Content";
            var post = await _dbContext.Posts.FirstAsync();
            post.Title = newTitle;
            post.Content = newContent;

            var updated = await _postService
                .UpdatePostAsync(post);

            updated.Should().BeTrue();

            var updatedPost = await _postService
                .GetPostByIdAsync(post.Id);

            updatedPost.Title.Should().Be(newTitle);
            updatedPost.Content.Should().Be(newContent);
            updatedPost.UpdatedAt.Should().Be(DateTime.Now.DefaultFormat());
        }

        #endregion

        #region Delete Post

        [Fact]
        public async Task ShouldBeReturnedFalseIfDeletedPostNotExists()
        {
            var notExistingPost = new PostBuilder()
                .WithTitle("Post not found")
                .WithContent("It's a post not exist")
                .WithAttachments("err.txt,image.jpg")
                .Build();

            var deleted = await _postService
                .DeletePostAsync(notExistingPost);

            deleted.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldBeReturnedFalseIfDeletedPostIdNotExists()
        {
            var notExistingId = Guid.NewGuid();

            var deleted = await _postService
                .DeletePostAsync(notExistingId);

            deleted.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldBeDeletedPost()
        {
            var lastPost = await _dbContext
                .Posts
                .LastAsync();

            var deleted = await _postService
                .DeletePostAsync(lastPost);

            deleted.Should().BeTrue();
            _dbContext.Posts.Should().HaveCount(1);

            var deletedPost = await _postService
                .GetPostByIdAsync(lastPost.Id);

            deletedPost.Should().BeNull();
        }

        [Fact]
        public async Task ShouldBeDeletedAuthorById()
        {
            var firstPost = await _dbContext
                .Posts
                .FirstAsync();

            var deleted = await _postService
                .DeletePostAsync(firstPost.Id);

            deleted.Should().BeTrue();
            _dbContext.Posts.Should().HaveCount(1);

            var deletedPost = await _postService
                .GetPostByIdAsync(firstPost.Id);

            deletedPost.Should().BeNull();
        }

        #endregion
    }
}
