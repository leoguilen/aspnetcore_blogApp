using Bogus;
using FluentAssertions;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request.Author;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Author;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medium.IntegrationTest.Controllers.AuthorControllerTest
{
    public class UpdateTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Authors.Update;
        private readonly Guid _requestId = Guid
            .Parse("d4182477-0823-4908-be1d-af808e594306");

        private readonly ITestOutputHelper _output;
        private readonly UpdateAuthorRequest _updateAuthorRequest;
        private readonly Faker _faker;

        public UpdateTest(CustomWebApplicationFactory factory, 
            ITestOutputHelper output) : base(factory)
        {
            _faker = new Faker("pt_BR");
            _output = output;

            _updateAuthorRequest = new UpdateAuthorRequest
            {
                FirstName = _faker.Person.FirstName,
                LastName = _faker.Person.LastName,
                Username = _faker.Person.UserName,
                Email = _faker.Person.Email,
                Avatar = _faker.Person.Avatar,
                Bio = _faker.Lorem.Text()
            };
        }

        [Fact]
        public async Task ShouldBeReturned_NotFoundResponse_IfAuthorIdNotExists()
        {
            await AuthenticateAsync();

            var randomId = Guid.NewGuid().ToString();

            var response = await HttpClientTest.PutAsJsonAsync(
                _requestUri.Replace("{authorId}", randomId), _updateAuthorRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldBeReturned_ErrorResponse_IfAnyFieldIsInvalid()
        {
            await AuthenticateAsync();

            _updateAuthorRequest.Email = _faker.Random.String2(15);

            var response = await HttpClientTest.PutAsJsonAsync(
                _requestUri.Replace("{authorId}", _requestId.ToString()),
                    _updateAuthorRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();

            JObject jContent = JObject.Parse(content);
            jContent.Value<string>("title").Should()
                .Be("One or more validation errors occurred.");
            jContent.GetValue("errors")["Email"].HasValues.Should()
                .BeTrue();
        }

        [Fact]
        public async Task ShouldBeReturned_SuccessResponse_AndUpdatedAuthorInDatabase()
        {
            await AuthenticateAsync();

            var response = await HttpClientTest.PutAsJsonAsync(
                _requestUri.Replace("{authorId}", _requestId.ToString()), 
                    _updateAuthorRequest);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<Response<AuthorResponse>>())
                .Data.Should().BeEquivalentTo(new Response<AuthorResponse>(
                    new AuthorResponse 
                    {
                        Id = _requestId,
                        FirstName = _updateAuthorRequest.FirstName,
                        LastName = _updateAuthorRequest.LastName,
                        Email = _updateAuthorRequest.Email,
                        Username = _updateAuthorRequest.Username
                    }), x => x.ExcludingMissingMembers());
        }
    }
}
