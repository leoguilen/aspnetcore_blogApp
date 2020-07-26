using FluentAssertions;
using Medium.Core.Contracts.V1;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Medium.IntegrationTest.Controllers.TagControllerTest
{
    public class DeleteTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Tags.Delete;

        public DeleteTest(CustomWebApplicationFactory factory) 
            : base(factory) {}

        [Fact]
        public async Task ShouldBeReturned_NotFoundResponse_IfTagIdNotExists()
        {
            await AuthenticateAsync();

            var randomId = Guid.NewGuid().ToString();

            var response = await HttpClientTest.DeleteAsync(
                _requestUri.Replace("{tagId}", randomId));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldBeReturned_SuccessResponse_AndDeleteTagInTheDatabase()
        {
            await AuthenticateAsync();

            var tagId = "5d5e9a28-7c3e-4c2a-8098-b866eab33e61";

            var response = await HttpClientTest.DeleteAsync(
                _requestUri.Replace("{tagId}", tagId));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var deletedTagResponse = await HttpClientTest.GetAsync(
                ApiRoutes.Tags.Get.Replace("{tagId}", tagId));

            deletedTagResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
