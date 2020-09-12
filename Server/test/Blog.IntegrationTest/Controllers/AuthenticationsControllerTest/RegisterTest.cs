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
    public class RegisterTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Authentication.Register;

        private readonly ITestOutputHelper _output;
        private readonly AuthorRegistrationRequest _authorRegistrationhorRequest;
        private readonly Faker _faker;

        public RegisterTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _output = output;
            _faker = new Faker("pt_BR");

            _authorRegistrationhorRequest = new AuthorRegistrationRequest
            {
                FirstName = _faker.Person.FirstName,
                LastName = _faker.Person.LastName,
                Username = _faker.Person.UserName,
                Email = _faker.Person.Email,
                Password = _faker.Internet.Password(memorable: true, prefix: "Y#10")
            };
        }

        #region Validations

        [Theory]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("Joao123")]
        public async Task ShouldBeReturned_ErrorResponse_IfFirstNameFieldIsNotValidated(string invalidFirstName)
        {
            _authorRegistrationhorRequest.FirstName = invalidFirstName;

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorRegistrationhorRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            JObject jContent = JObject.Parse(content);

            jContent.Value<string>("title").Should()
                .Be("One or more validation errors occurred.");
            jContent.GetValue("errors")["FirstName"].HasValues.Should()
                .BeTrue();

            _output.WriteLine($"Valor entrada: {invalidFirstName} | Resultado teste: {jContent.GetValue("errors")["FirstName"].First}");
        }

        [Theory]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("Silv123")]
        public async Task ShouldBeReturned_ErrorResponse_IfLastNameFieldIsNotValidated(string invalidLastName)
        {
            _authorRegistrationhorRequest.LastName = invalidLastName;

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorRegistrationhorRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            JObject jContent = JObject.Parse(content);

            jContent.Value<string>("title").Should()
                .Be("One or more validation errors occurred.");
            jContent.GetValue("errors")["LastName"].HasValues.Should()
                .BeTrue();

            _output.WriteLine($"Valor entrada: {invalidLastName} | Resultado teste: {jContent.GetValue("errors")["LastName"].First}");
        }

        [Fact]
        public async Task ShouldBeReturned_ErrorResponse_IfUsernameFieldIsNotValidated()
        {
            _authorRegistrationhorRequest.Username = string.Empty;

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorRegistrationhorRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            JObject jContent = JObject.Parse(content);

            jContent.Value<string>("title").Should()
                .Be("One or more validation errors occurred.");
            jContent.GetValue("errors")["Username"].HasValues.Should()
                .BeTrue();

            _output.WriteLine($"Resultado teste: {jContent.GetValue("errors")["Username"].First}");
        }

        [Theory]
        [InlineData("")]
        [InlineData("123abc")]
        public async Task ShouldBeReturned_ErrorResponse_IfPasswordFieldIsNotValidated(string invalidPassword)
        {
            _authorRegistrationhorRequest.Password = invalidPassword;

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorRegistrationhorRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            JObject jContent = JObject.Parse(content);

            jContent.Value<string>("title").Should()
                .Be("One or more validation errors occurred.");
            jContent.GetValue("errors")["Password"].HasValues.Should()
                .BeTrue();

            _output.WriteLine($"Valor entrada: {invalidPassword} | Resultado teste: {jContent.GetValue("errors")["Password"].First}");
        }

        [Fact]
        public async Task ShouldBeReturned_ErrorResponse_IfEmailFieldIsNotValidated()
        {
            var invalidEmail = _faker.Random.String2(10, 20);
            _authorRegistrationhorRequest.Email = invalidEmail;

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorRegistrationhorRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            JObject jContent = JObject.Parse(content);

            jContent.Value<string>("title").Should()
                .Be("One or more validation errors occurred.");
            jContent.GetValue("errors")["Email"].HasValues.Should()
                .BeTrue();

            _output.WriteLine($"Valor entrada: {invalidEmail} | Resultado teste: {jContent.GetValue("errors")["Email"].First}");
        }

        #endregion

        [Theory]
        [InlineData("joao@email.com")]
        [InlineData("maria@email.com")]
        public async Task ShouldBeReturned_FailedResponse_IfAlreadyExistsEqualsEmail(string alreadyRegisteredEmail)
        {
            _authorRegistrationhorRequest.Email = alreadyRegisteredEmail;

            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorRegistrationhorRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadAsAsync<AuthFailedResponse>())
                .Errors.Should()
                .HaveCount(1).And
                .BeEquivalentTo(new[] { "Author with this email already exists" });

            _output.WriteLine($"Valor entrada: {alreadyRegisteredEmail} | Resultado teste: {response.StatusCode}");
        }

        [Fact]
        public async Task ShouldBeReturned_SuccessResponse_AndRegisterNewAuthor()
        {
            var response = await HttpClientTest
                .PostAsJsonAsync(_requestUri,
                    _authorRegistrationhorRequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<AuthSuccessResponse>())
                .Token.Should().NotBeNullOrEmpty();

            _output.WriteLine($"Valor entrada: {JObject.FromObject(_authorRegistrationhorRequest)} | Resultado teste: {response.StatusCode}");
        }
    }
}
