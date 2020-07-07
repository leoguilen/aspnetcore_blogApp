using Medium.Core.Domain;
using System.Threading.Tasks;

namespace Medium.Core.Repositories
{
    public interface IAuthorAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}
