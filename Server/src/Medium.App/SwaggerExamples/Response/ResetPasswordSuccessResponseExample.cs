using Medium.Core.Contracts.V1.Request;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Response
{
    public class ResetPasswordSuccessResponseExample : IExamplesProvider<ResetPasswordSuccessResponse>
    {
        public ResetPasswordSuccessResponse GetExamples()
        {
            return new ResetPasswordSuccessResponse
            {
                Reseted = true,
                SuccessMessage = "Senha resetada com sucesso"
            };
        }
    }
}
