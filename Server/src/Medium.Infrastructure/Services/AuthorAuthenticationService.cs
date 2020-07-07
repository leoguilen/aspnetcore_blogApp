using Medium.Core.Domain;
using Medium.Core.Options;
using Medium.Core.Repositories;
using Medium.Core.UnitOfWork;
using Medium.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Medium.Infrastructure.Services
{
    public class AuthorAuthenticationService : IAuthorAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtOptions _jwtSettings;

        public AuthorAuthenticationService(IUnitOfWork unitOfWork, JwtOptions jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var author = await _unitOfWork.Authors
                .FindByConditionAsync(a => a.Email == email)
                .Result.SingleOrDefaultAsync();

            if (author == null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "User does not exist" }
                };
            }

            bool isValidPassword = SecurePasswordHasher
                .AreEqual(password, author.Password, author.Salt);

            if (!isValidPassword)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "Email/password combination is invalid" }
                };
            }

            return new AuthenticationResult
            {
                Success = true
            };
        }
    }
}
