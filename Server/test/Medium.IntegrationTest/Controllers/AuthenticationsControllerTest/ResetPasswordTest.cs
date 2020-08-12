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
    public class ResetPasswordTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Authentication.ResetPassword;

        private readonly ITestOutputHelper _output;
        private readonly AuthorResetPasswordRequest _authorResetPasswordRequest;
        private readonly Faker _faker;

        public ResetPasswordTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _output = output;
            _faker = new Faker("pt_BR");

            _authorResetPasswordRequest = new AuthorResetPasswordRequest
            {
                Email = "maria@email.com",
                NewPassword = _faker.Internet
                    .Password(memorable: true, prefix: "Y#10")
            };
        }

        [Fact]
        public async Task ShouldBeReturned_FailedAuthResponse_IfEmailIsInvalid()
        {
            _authorResetPasswordRequest.Email = _faker.Person.Email;

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorResetPasswordRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadAsAsync<AuthFailedResponse>())
                .Errors.Should()
                .HaveCount(1).And
                .BeEquivalentTo(new[] { "Email does not exist" });

            _output.WriteLine($"Valor entrada: {_authorResetPasswordRequest.Email} | Resultado teste: {response.StatusCode}");
        }

        [Fact]
        public async Task ShouldBeReturned_FailedAuthResponse_IfPasswordNotMatchThePatterns()
        {
            _authorResetPasswordRequest.NewPassword = _faker.Random.AlphaNumeric(10);

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorResetPasswordRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            JObject jContent = JObject.Parse(content);

            jContent.Value<string>("title").Should()
                .Be("One or more validation errors occurred.");
            jContent.GetValue("errors")["NewPassword"].HasValues.Should()
                .BeTrue();

            _output.WriteLine($"Valor entrada: {_authorResetPasswordRequest.NewPassword} | Resultado teste: {response.StatusCode}");
        }

        [Fact]
        public async Task ShouldBeReturned_SuccessResetPasswordResponse()
        {
            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorResetPasswordRequest);

            var content = await response.Content
                .ReadAsAsync<ResetPasswordSuccessResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Reseted.Should().BeTrue();
            content.SuccessMessage.Should()
                .Be("Senha redefinida com sucesso");
        }
    }
}
