using FluentAssertions;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Tag;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medium.IntegrationTest.Controllers.TagControllerTest
{
    public class GetByIdTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Tags.Get;
        private readonly ITestOutputHelper _output;

        public GetByIdTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldBeReturned_NotFoundResponse_IfTagIdNotExistsInDatabase()
        {
            var randomId = Guid.NewGuid().ToString();

            await AuthenticateAsync();

            var response = await HttpClientTest.GetAsync(
                _requestUri.Replace("{tagId}", randomId));

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldBeReturned_TagResponse_IfTagIdExistsInDatabase()
        {
            var validId = Guid.Parse("5d5e9a28-7c3e-4c2a-8098-b866eab33e61");
            var expectedTagResponse =
                new Response<TagResponse>(
                    new TagResponse
                    {
                        Id = validId,
                        Name = "Tag_1"
                    });

            await AuthenticateAsync();

            var response = await HttpClientTest.GetAsync(
                _requestUri.Replace("{tagId}",
                    validId.ToString()));

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<Response<TagResponse>>())
                .Data.Should()
                .NotBeNull().And
                .BeEquivalentTo(expectedTagResponse,
                    x => x.ExcludingMissingMembers());
        }
    }
}
