using Medium.Core.Contracts.V1.Request.Authentication;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Request
{
    public class AuthorResetPasswordRequestExample : IExamplesProvider<AuthorResetPasswordRequest>
    {
        public AuthorResetPasswordRequest GetExamples()
        {
            return new AuthorResetPasswordRequest
            {
                Email = "example@email.com",
                NewPassword = "Example321#"
            };
        }
    }
}
