using Bogus;
using FluentAssertions;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request.Tag;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Tag;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medium.IntegrationTest.Controllers.TagControllerTest
{
    public class CreateTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Tags.Create;

        private readonly CreateTagRequest _createTagRequest;
        private readonly ITestOutputHelper _output;
        private readonly Faker _faker;

        public CreateTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _faker = new Faker("pt_BR");
            _output = output;

            _createTagRequest = new CreateTagRequest
            {
                Name = _faker.Random.String2(8),
            };
        }

        [Fact]
        public async Task ShouldBeReturned_ErrorResponse_IfNameIsNotValidated()
        {
            await AuthenticateAsync();

            _createTagRequest.Name = _faker.Random.String2(0, 3);

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _createTagRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            JObject jContent = JObject.Parse(content);

            jContent.Value<string>("title").Should()
                .Be("One or more validation errors occurred.");
            jContent.GetValue("errors")["Name"].HasValues.Should()
                .BeTrue();
        }

        [Fact]
        public async Task ShouldBeReturned_SuccessResponse_AndRegisterNewTag()
        {
            await AuthenticateAsync();

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _createTagRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            (await response.Content.ReadAsAsync<Response<TagResponse>>())
                .Data.Should().BeEquivalentTo(new Response<TagResponse>(
                    new TagResponse
                    {
                        Name = _createTagRequest.Name
                    }), x => x.ExcludingMissingMembers());
        }
    }
}
