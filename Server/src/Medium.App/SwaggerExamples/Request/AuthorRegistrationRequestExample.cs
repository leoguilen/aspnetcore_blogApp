using Medium.Core.Contracts.V1.Request;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Request
{
    public class AuthorRegistrationRequestExample : IExamplesProvider<AuthorRegistrationRequest>
    {
        public AuthorRegistrationRequest GetExamples()
        {
            return new AuthorRegistrationRequest
            {
                FirstName = "example",
                LastName = "swagger",
                Username = "example.swagger",
                Email = "example@email.com",
                Password = "Example123#"
            };
        }
    }
}
