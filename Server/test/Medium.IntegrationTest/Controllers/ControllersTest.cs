using System.Net.Http;
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
    }
}
