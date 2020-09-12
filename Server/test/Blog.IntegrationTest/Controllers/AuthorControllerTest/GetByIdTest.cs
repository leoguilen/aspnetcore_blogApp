using FluentAssertions;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Author;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medium.IntegrationTest.Controllers.AuthorControllerTest
{
    public class GetByIdTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Authors.Get;
        private readonly ITestOutputHelper _output;

        public GetByIdTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldBeReturned_NotFoundResponse_IfAuthorIdNotExistsInDatabase()
        {
            var randomId = Guid.NewGuid().ToString();

            await AuthenticateAsync();

            var response = await HttpClientTest.GetAsync(
                _requestUri.Replace("{authorId}", randomId));

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldBeReturned_AuthorResponse_IfAuthorIdExistsInDatabase()
        {
            var validId = Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702");
            var expectedAuthorResponse =
                new Response<AuthorResponse>(
                    new AuthorResponse
                    {
                        Id = validId,
                        FirstName = "Maria",
                        Email = "maria@email.com"
                    });

            await AuthenticateAsync();

            var response = await HttpClientTest.GetAsync(
                _requestUri.Replace("{authorId}",
                    validId.ToString()));

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<Response<AuthorResponse>>())
                .Data.Should()
                .NotBeNull().And
                .BeEquivalentTo(expectedAuthorResponse,
                    x => x.ExcludingMissingMembers());
        }
    }
}
