using Medium.Core.Contracts.V1.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Response
{
    public class AuthSuccessResponseExample : IExamplesProvider<AuthSuccessResponse>
    {
        public AuthSuccessResponse GetExamples()
        {
            return new AuthSuccessResponse
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9eyJz" +
                        "dWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG" +
                        "9lIiwiaWF0IjoxNTE2MjM5MDIyfQSflKxwRJSMeKKF2Q" +
                        "T4fwpMeJf36POk6yJV_adQssw5c"
            };
        }
    }
}
