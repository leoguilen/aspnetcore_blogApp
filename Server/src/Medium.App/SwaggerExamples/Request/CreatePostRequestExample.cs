using Medium.Core.Contracts.V1.Request;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Request
{
    public class CreatePostRequestExample : IExamplesProvider<CreatePostRequest>
    {
        public CreatePostRequest GetExamples()
        {
            return new CreatePostRequest
            {
                Title = "Post",
                Content = "Post to example",
                Attachments = "example.jpg;example.gif"
            };
        }
    }
}
