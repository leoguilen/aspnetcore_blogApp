using FluentAssertions;
using Medium.Core.Contracts.V1;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Medium.IntegrationTest.Controllers.PostControllerTest
{
    public class DeleteTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Posts.Delete;
        
        public DeleteTest(CustomWebApplicationFactory factory)
            : base(factory) { }

        [Fact]
        public async Task ShouldBeReturned_NotFoundResponse_IfPostIdNotExists()
        {
            await AuthenticateAsync();

            var randomId = Guid.NewGuid().ToString();

            var response = await HttpClientTest.DeleteAsync(
                _requestUri.Replace("{postId}", randomId));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldBeReturned_SuccessResponse_AndDeletePostInTheDatabase()
        {
            await AuthenticateAsync();

            var postId = "a06ba60c-c999-4de3-aa23-4f0c13bd71ad";

            var response = await HttpClientTest.DeleteAsync(
                _requestUri.Replace("{postId}", postId));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var deletedPostResponse = await HttpClientTest.GetAsync(
                ApiRoutes.Posts.Get.Replace("{postId}", postId));

            deletedPostResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
