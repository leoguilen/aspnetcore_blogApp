using Medium.Core.Common.Builder;
using Medium.Core.Contracts.V1.Request.Author;

namespace Medium.App.Extensions
{
    public static class AuthorsControllerExtensions
    {
        public static AuthorBuilder BuildAuthorByRequest(this AuthorBuilder authorBuilder, CreateAuthorRequest request)
        {
            return authorBuilder?
                .WithFirstName(request?.FirstName)
                .WithLastName(request?.LastName)
                .WithUsername(request?.Username)
                .WithPassword(request?.Password)
                .WithEmail(request?.Email)
                .WithBio(request?.Bio)
                .WithAvatar(request?.Avatar);
        }
    }
}
