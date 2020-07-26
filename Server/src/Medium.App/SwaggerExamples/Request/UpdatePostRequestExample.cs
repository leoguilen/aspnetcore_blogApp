using Medium.Core.Contracts.V1.Request.Post;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Request
{
    public class UpdatePostRequestExample : IExamplesProvider<UpdatePostRequest>
    {
        public UpdatePostRequest GetExamples()
        {
            return new UpdatePostRequest
            {
                Title = "Updated Title",
                Content = "Updated Content",
                Attachments = "updated.png"
            };
        }
    }
}
