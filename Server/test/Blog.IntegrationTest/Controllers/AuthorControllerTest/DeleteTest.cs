using FluentAssertions;
using Medium.Core.Contracts.V1;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medium.IntegrationTest.Controllers.AuthorControllerTest
{
    public class DeleteTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Authors.Delete;
        private readonly ITestOutputHelper _output;

        public DeleteTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldBeReturned_NotFoundResponse_IfAuthorIdNotExists()
        {
            await AuthenticateAsync();

            var randomId = Guid.NewGuid().ToString();

            var response = await HttpClientTest.DeleteAsync(
                _requestUri.Replace("{authorId}", randomId));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldBeReturned_SuccessResponse_AndDeleteAuthorInTheDatabase()
        {
            await AuthenticateAsync();

            var authorId = "d4182477-0823-4908-be1d-af808e594306";

            var response = await HttpClientTest.DeleteAsync(
                _requestUri.Replace("{authorId}", authorId));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var deletedAuthorResponse = await HttpClientTest.GetAsync(
                ApiRoutes.Authors.Get.Replace("{authorId}", authorId));

            deletedAuthorResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
