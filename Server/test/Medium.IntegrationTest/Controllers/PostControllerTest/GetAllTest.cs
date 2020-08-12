using FluentAssertions;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Post;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medium.IntegrationTest.Controllers.PostControllerTest
{
    public class GetAllTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Posts.GetAll;
        private readonly ITestOutputHelper _output;

        public GetAllTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldBeReturned_AllPosts_InTheDatabase()
        {
            await AuthenticateAsync();

            var response = await HttpClientTest
                .GetAsync(_requestUri);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<PagedResponse<PostResponse>>())
                .Data.Should()
                .NotBeNullOrEmpty().And
                .SatisfyRespectively(
                    post1 =>
                    {
                        post1.Id.Should().Be(Guid.Parse("b65afc54-d766-4377-8c89-22662582174e"));
                        post1.Title.Should().Be("Post 1");
                        post1.Content.Should().Be("First post content");
                        post1.Attachments.Should().Be("post1img1.jpg,post1img2.jpg");
                        post1.AuthorId.Should().Be(Guid.Parse("d4182477-0823-4908-be1d-af808e594306"));
                    },
                    post2 =>
                    {
                        post2.Id.Should().Be(Guid.Parse("a06ba60c-c999-4de3-aa23-4f0c13bd71ad"));
                        post2.Title.Should().Be("Post 2");
                        post2.Content.Should().Be("Second post content");
                        post2.Attachments.Should().Be("post2img1.jpg,post2img2.jpg");
                        post2.AuthorId.Should().Be(Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702"));
                    });
        }
    }
}
