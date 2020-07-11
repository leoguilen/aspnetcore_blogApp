using Medium.Core.Contracts.V1.Request;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Request
{
    public class AuthorLoginRequestExample : IExamplesProvider<AuthorLoginRequest>
    {
        public AuthorLoginRequest GetExamples()
        {
            return new AuthorLoginRequest
            {
                Email = "example@email.com",
                Password = "Example123#"
            };
        }
    }
}
