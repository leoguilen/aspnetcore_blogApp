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
    public class GetAllTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Tags.GetAll;
        private readonly ITestOutputHelper _output;

        public GetAllTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldBeReturned_AllTags_InTheDatabase()
        {
            await AuthenticateAsync();

            var response = await HttpClientTest
                .GetAsync(_requestUri);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<PagedResponse<TagResponse>>())
                .Data.Should()
                .NotBeNullOrEmpty().And
                .SatisfyRespectively(
                    tag1 =>
                    {
                        tag1.Id.Should().Be(Guid.Parse("5d5e9a28-7c3e-4c2a-8098-b866eab33e61"));
                        tag1.Name.Should().Be("Tag_1");
                    },
                    tag2 =>
                    {
                        tag2.Id.Should().Be(Guid.Parse("d94e6e00-96d0-4fc7-b621-c7746705b471"));
                        tag2.Name.Should().Be("Tag_2");
                    });
        }
    }
}
