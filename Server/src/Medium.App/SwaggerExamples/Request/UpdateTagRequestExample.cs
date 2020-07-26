using Medium.Core.Contracts.V1.Request.Tag;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Request
{
    public class UpdateTagRequestExample : IExamplesProvider<UpdateTagRequest>
    {
        public UpdateTagRequest GetExamples()
        {
            return new UpdateTagRequest
            {
                Name = "Updated_Tag"
            };
        }
    }
}
