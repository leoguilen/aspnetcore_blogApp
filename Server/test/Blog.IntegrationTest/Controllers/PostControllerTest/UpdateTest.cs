using Bogus;
using FluentAssertions;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request.Post;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Post;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medium.IntegrationTest.Controllers.PostControllerTest
{
    public class UpdateTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Posts.Update;
        private readonly Guid _requestId = Guid
            .Parse("b65afc54-d766-4377-8c89-22662582174e");

        private readonly ITestOutputHelper _output;
        private readonly UpdatePostRequest _updatePostRequest;
        private readonly Faker _faker;

        public UpdateTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _faker = new Faker("pt_BR");
            _output = output;

            _updatePostRequest = new UpdatePostRequest
            {
                Title = _faker.Random.String2(50),
                Content = _faker.Lorem.Text(),
                Attachments = _faker.Image.PicsumUrl()
            };
        }

        [Fact]
        public async Task ShouldBeReturned_NotFoundResponse_IfPostIdNotExists()
        {
            await AuthenticateAsync();

            var randomId = Guid.NewGuid().ToString();

            var response = await HttpClientTest.PutAsJsonAsync(
                _requestUri.Replace("{postId}", randomId), _updatePostRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldBeReturned_ErrorResponse_IfTitleIsInvalid()
        {
            await AuthenticateAsync();

            _updatePostRequest.Title = string.Empty;

            var response = await HttpClientTest.PutAsJsonAsync(
                _requestUri.Replace("{postId}", _requestId.ToString()),
                    _updatePostRequest);

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
        public async Task ShouldBeReturned_SuccessResponse_AndUpdatedAuthorInDatabase()
        {
            await AuthenticateAsync();

            var response = await HttpClientTest.PutAsJsonAsync(
                _requestUri.Replace("{postId}", _requestId.ToString()),
                    _updatePostRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<Response<PostResponse>>())
                .Data.Should().BeEquivalentTo(new Response<PostResponse>(
                    new PostResponse
                    {
                        Id = _requestId,
                        Title = _updatePostRequest.Title,
                        Content = _updatePostRequest.Content,
                        Attachments = _updatePostRequest.Attachments
                    }), x => x.ExcludingMissingMembers());
        }
    }
}
