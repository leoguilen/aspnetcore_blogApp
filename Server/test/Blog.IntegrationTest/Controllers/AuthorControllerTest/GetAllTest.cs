using FluentAssertions;
using Medium.Core.Contracts.V1;
using Medium.Core.Contracts.V1.Response;
using Medium.Core.Contracts.V1.Response.Author;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Medium.IntegrationTest.Controllers.AuthorControllerTest
{
    public class GetAllTest : ControllersTest
    {
        private readonly string _requestUri = ApiRoutes.Authors.GetAll;
        private readonly ITestOutputHelper _output;

        public GetAllTest(CustomWebApplicationFactory factory,
            ITestOutputHelper output) : base(factory)
        {
            _output = output;
        }

        [Fact]
        public async Task ShouldBeReturned_AllAuthors_InTheDatabase()
        {
            await AuthenticateAsync();

            var response = await HttpClientTest
                .GetAsync(_requestUri);

            _output.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<PagedResponse<AuthorResponse>>())
                .Data.Should()
                .NotBeNullOrEmpty().And
                .SatisfyRespectively(
                    author1 =>
                    {
                        author1.Id.Should().Be(Guid.Parse("d4182477-0823-4908-be1d-af808e594306"));
                        author1.FirstName.Should().Be("João");
                        author1.Email.Should().Be("joao@email.com");
                    },
                    author2 =>
                    {
                        author2.Id.Should().Be(Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702"));
                        author2.FirstName.Should().Be("Maria");
                        author2.Email.Should().Be("maria@email.com");
                    });
        }
    }
}
