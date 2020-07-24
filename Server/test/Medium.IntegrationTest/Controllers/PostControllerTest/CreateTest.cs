using Bogus;
using FluentAssertions;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request.Post;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Post;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medium.IntegrationTest.Controllers.PostControllerTest
{
    public class CreateTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Posts.Create;

        private readonly CreatePostRequest _createPostRequest;
        private readonly ITestOutputHelper _output;
        private readonly Faker _faker;

        public CreateTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _faker = new Faker("pt_BR");
            _output = output;

            _createPostRequest = new CreatePostRequest
            {
                Title = _faker.Random.String2(50),
                Content = _faker.Lorem.Text(),
                Attachments = _faker.Image.PicsumUrl()
            };
        }

        [Fact]
        public async Task ShouldBeReturned_ErrorResponse_IfTitleIsNotValidated()
        {
            await AuthenticateAsync();

            _createPostRequest.Title = _faker.Random.String2(51, 100);

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _createPostRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            JObject jContent = JObject.Parse(content);

            jContent.Value<string>("title").Should()
                .Be("One or more validation errors occurred.");
            jContent.GetValue("errors")["Title"].HasValues.Should()
                .BeTrue();
        }

        [Fact]
        public async Task ShouldBeReturned_SuccessResponse_AndRegisterNewPost()
        {
            await AuthenticateAsync();

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _createPostRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            (await response.Content.ReadAsAsync<Response<PostResponse>>())
                .Data.Should().BeEquivalentTo(new Response<PostResponse>(
                    new PostResponse
                    {
                        Title = _createPostRequest.Title,
                        Content = _createPostRequest.Content,
                        Attachments = _createPostRequest.Attachments
                    }), x => x.ExcludingMissingMembers());
        }
    }
}
