using Medium.Core.Contracts.V1.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Response
{
    public class AuthFailedResponseExample : IExamplesProvider<AuthFailedResponse>
    {
        public AuthFailedResponse GetExamples()
        {
            return new AuthFailedResponse
            {
                Errors = new[] { "Email inválido" }
            };
        }
    }
}
