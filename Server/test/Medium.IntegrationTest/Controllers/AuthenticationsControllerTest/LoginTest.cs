using Bogus;
using FluentAssertions;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request.Authentication;
using Medium.Core.Contracts.V1.Response.Authentication;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medium.IntegrationTest.Controllers.AuthenticationsControllerTest
{
    public class LoginTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Authentication.Login;

        private readonly ITestOutputHelper _output;
        private readonly AuthorLoginRequest _authorLoginRequest;
        private readonly Faker _faker;

        public LoginTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _output = output;
            _faker = new Faker("pt_BR");

            _authorLoginRequest = new AuthorLoginRequest
            {
                Email = "joao@email.com",
                Password = "joao123"
            };
        }

        [Fact]
        public async Task ShouldBeReturned_FailerAuthResponse_IfEmailIsInvalid()
        {
            _authorLoginRequest.Email = _faker.Person.Email;

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorLoginRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadAsAsync<AuthFailedResponse>())
                .Errors.Should()
                .HaveCount(1).And
                .BeEquivalentTo(new[] { "Author does not exist" });

            _output.WriteLine($"Valor entrada: {_authorLoginRequest.Email} | Resultado teste: {response.StatusCode}");
        }

        [Fact]
        public async Task ShouldBeReturned_FailerAuthResponse_IfPasswordIsInvalid()
        {
            _authorLoginRequest.Password = _faker.Internet.Password();

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorLoginRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadAsAsync<AuthFailedResponse>())
                .Errors.Should()
                .HaveCount(1).And
                .BeEquivalentTo(new[] { "Email/password combination is invalid" });

            _output.WriteLine($"Valor entrada: {_authorLoginRequest.Password} | Resultado teste: {response.StatusCode}");
        }

        [Fact]
        public async Task ShouldBeReturned_SuccessAuthResponse_IfCredentialsIsValid()
        {
            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorLoginRequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<AuthSuccessResponse>())
                .Token.Should().NotBeNullOrEmpty();

            _output.WriteLine($"Valor entrada: {JObject.FromObject(_authorLoginRequest)} | Resultado teste: {response.StatusCode}");
        }
    }
}
