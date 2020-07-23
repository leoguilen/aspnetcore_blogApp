using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Request.Authentication;
using Medium.Core.Contracts.V1.Response.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Medium.IntegrationTest.Controllers
{
    public class ControllersTest : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly HttpClient HttpClientTest;

        public ControllersTest(CustomWebApplicationFactory factory)
        {
            HttpClientTest = factory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            HttpClientTest.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await HttpClientTest.PostAsJsonAsync(
                ApiRoutes.Authentication.Login,
                new AuthorLoginRequest
                {
                    Email = "maria@email.com",
                    Password = "maria123"
                });

            var authResponse = await response.Content
                .ReadAsAsync<AuthSuccessResponse>();

            return authResponse.Token;
        }
    }
}
