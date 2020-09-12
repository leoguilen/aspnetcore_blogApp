using Medium.Core.Domain;
using System.Threading.Tasks;

namespace Medium.Core.Services
{
    public interface IAuthorAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RegisterAsync(Author author);
        Task<AuthenticationResult> ResetPasswordAsync(string email, string newPassword);
    }
}
