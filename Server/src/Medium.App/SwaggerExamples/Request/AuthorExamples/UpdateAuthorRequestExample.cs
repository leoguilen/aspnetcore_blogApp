using Medium.Core.Contracts.V1.Request.Author;
using Swashbuckle.AspNetCore.Filters;

namespace Medium.App.SwaggerExamples.Request.AuthorExamples
{
    public class UpdateAuthorRequestExample : IExamplesProvider<UpdateAuthorRequest>
    {
        public UpdateAuthorRequest GetExamples()
        {
            return new UpdateAuthorRequest
            {
                FirstName = "Updated FirstName",
                LastName = "Updated LastName",
                Username = "Updated Username",
                Email = "Updated Email",
                Avatar = "Updated Avatar",
                Bio = "Updated Bio"
            };
        }
    }
}
