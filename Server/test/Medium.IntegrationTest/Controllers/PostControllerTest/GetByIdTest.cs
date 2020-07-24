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
    public class GetByIdTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Posts.Get;
        private readonly ITestOutputHelper _output;

        public GetByIdTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldBeReturned_NotFoundResponse_IfPostIdNotExistsInDatabase()
        {
            var randomId = Guid.NewGuid().ToString();

            await AuthenticateAsync();

            var response = await HttpClientTest.GetAsync(
                _requestUri.Replace("{postId}", randomId));

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldBeReturned_PostResponse_IfAuthorIdExistsInDatabase()
        {
            var validId = Guid.Parse("b65afc54-d766-4377-8c89-22662582174e");
            var expectedPostResponse =
                new Response<PostResponse>(
                    new PostResponse
                    {
                        Id = validId,
                        Title = "Post 1",
                        Content = "First post content",
                        Attachments = "post1img1.jpg,post1img2.jpg"
                    });

            await AuthenticateAsync();

            var response = await HttpClientTest.GetAsync(
                _requestUri.Replace("{postId}",
                    validId.ToString()));

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<Response<PostResponse>>())
                .Data.Should()
                .NotBeNull().And
                .BeEquivalentTo(expectedPostResponse,
                    x => x.ExcludingMissingMembers());
        }
    }
}
