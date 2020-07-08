using Medium.Core.Domain;
using Medium.Core.Options;
using Medium.Core.Services;
using Medium.Core.UnitOfWork;
using Medium.Infrastructure.Helpers;
using Medium.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Medium.Infrastructure.Services
{
    public class AuthorAuthenticationService : IAuthorAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtOptions _jwtOptions;

        public AuthorAuthenticationService(IUnitOfWork unitOfWork, JwtOptions jwtOptions)
        {
            _unitOfWork = unitOfWork;
            _jwtOptions = jwtOptions;
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
                    Errors = new[] { "Author does not exist" }
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

            return await TokenUtils.GenerateAuthenticationResultForUserAsync(author, _jwtOptions);
        }

        public async Task<AuthenticationResult> RegisterAsync(Author author)
        {
            var existingAuthor = await _unitOfWork.Authors
                .FindByConditionAsync(a => a.Email == author.Email)
                .Result.SingleOrDefaultAsync();

            if (existingAuthor != null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "Author with this email already exists" }
                };
            }

            string salt = SecurePasswordHasher.CreateSalt(8);
            string hashedPassword = SecurePasswordHasher.GenerateHash(author.Password, salt);

            author.Id = Guid.NewGuid();
            author.Password = hashedPassword;
            author.Salt = salt;

            await _unitOfWork.Authors.CreateAuthorAsync(author);
            var createdAuthor = await _unitOfWork.Commit();

            if (createdAuthor <= 0)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "An error occurred when try inserted new author" }
                };
            }

            return await TokenUtils.GenerateAuthenticationResultForUserAsync(author, _jwtOptions);
        }

        public async Task<AuthenticationResult> ResetPasswordAsync(string email, string newPassword)
        {
            var existingAuthor = await _unitOfWork.Authors
                .FindByConditionAsync(a => a.Email == email)
                .Result.SingleOrDefaultAsync();

            if (existingAuthor == null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "Email does not exist" }
                };
            }

            if (existingAuthor.Deleted)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "Email does not exist" }
                };
            }

            string newPasswordHashed = SecurePasswordHasher.GenerateHash(newPassword, existingAuthor.Salt);

            existingAuthor.Password = newPasswordHashed;

            await _unitOfWork.Authors.UpdateAuthorAsync(existingAuthor);
            int updated = await _unitOfWork.Commit();

            if (updated <= 0)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "An error occurred when try updated author" }
                };
            }

            return new AuthenticationResult
            {
                Success = true
            };
        }
    }
}
