using Medium.Core.Contracts.V1.Request.Tag;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Request
{
    public class CreateTagRequestExample : IExamplesProvider<CreateTagRequest>
    {
        public CreateTagRequest GetExamples()
        {
            return new CreateTagRequest
            {
                Name = "Tag_Example"
            };
        }
    }
}
